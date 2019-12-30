using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using TeamCityRetryTests.TeamCity.Connection;

namespace TeamCityRetryTests.TeamCity.ActionTypes
{
    internal class BuildArtifacts : IBuildArtifacts
    {
        private readonly ITeamCityCaller _mCaller;

        public BuildArtifacts(ITeamCityCaller caller)
        {
            _mCaller = caller;
        }

        public void DownloadArtifactsByBuildId(string buildId, Action<string> downloadHandler)
        {
            _mCaller.GetDownloadFormat(downloadHandler, "/downloadArtifacts.html?buildId={0}", false, buildId);
        }

        public ArtifactWrapper ByBuildConfigId(string buildConfigId, string param = "")
        {
            return new ArtifactWrapper(_mCaller, buildConfigId, param);
        }
    }

    public class ArtifactWrapper
    {
        private readonly ITeamCityCaller _mCaller;
        private readonly string _mBuildConfigId;
        private readonly string _mParam;

        internal ArtifactWrapper(ITeamCityCaller caller, string buildConfigId, string param)
        {
            _mCaller = caller;
            _mBuildConfigId = buildConfigId;
            _mParam = param;
        }

        public ArtifactCollection Tag(string tag)
        {
            return Specification(tag + ".tcbuildid");
        }

        public ArtifactCollection Specification(string buildSpecification)
        {
            var url = $"/repository/download/{_mBuildConfigId}/{buildSpecification}/teamcity-ivy.xml";
            var xml = _mCaller.GetRaw(string.IsNullOrEmpty(_mParam) ? url : $"{url}?{_mParam}", false);

            var document = new XmlDocument();
            document.LoadXml(xml);
            var artifactNodes = document.SelectNodes("//artifact");
            if (artifactNodes == null)
                return null;
            var list = new List<string>();
            foreach (XmlNode node in artifactNodes)
            {
                var nameNode = node.SelectSingleNode("@name");
                var extensionNode = node.SelectSingleNode("@ext");
                var artifact = string.Empty;
                if (nameNode != null)
                    artifact = nameNode.Value;
                if (extensionNode != null)
                    artifact += "." + extensionNode.Value;
                list.Add($"/repository/download/{_mBuildConfigId}/{buildSpecification}/{artifact}");
            }

            return new ArtifactCollection(_mCaller, list, _mParam);
        }
    }

    public class ArtifactCollection
    {
        private readonly ITeamCityCaller _mCaller;
        private readonly List<string> _mUrls;
        private readonly string _mParam;

        internal ArtifactCollection(ITeamCityCaller caller, List<string> urls, string param = "")
        {
            _mCaller = caller;
            _mUrls = urls;
            _mParam = param;
        }

        public List<string> GetArtifactUrl()
        {
            return _mUrls;
        }

        public List<string> Download(string directory = null, bool flatten = false, bool overwrite = true)
        {
            if (directory == null)
                directory = Directory.GetCurrentDirectory();
            var downloaded = new List<string>();
            foreach (var url in _mUrls)
            {
                Debug.Assert(url.StartsWith("/repository/download/"));

                var parts = url.Split('/').Skip(5).ToArray();
                var destination = flatten
                    ? parts.Last()
                    : string.Join(Path.DirectorySeparatorChar.ToString(), parts);
                destination = Path.Combine(directory, destination);

                var directoryName = Path.GetDirectoryName(destination);
                if (directoryName != null && !Directory.Exists(directoryName))
                    Directory.CreateDirectory(directoryName);

                downloaded.Add(Path.GetFullPath(destination));

                if (File.Exists(destination))
                {
                    if (overwrite) File.Delete(destination);
                    else continue;
                }

                var currentUrl = url;
                if (!string.IsNullOrEmpty(_mParam))
                {
                    currentUrl = $"{currentUrl}?{_mParam}";
                }

                _mCaller.GetDownloadFormat(tempfile => File.Move(tempfile, destination), currentUrl, false);
            }

            return downloaded;
        }

        public List<string> DownloadFiltered(string directory = null, List<string> filteredFiles = null,
            bool flatten = false, bool overwrite = true)
        {
            if (directory == null)
                directory = Directory.GetCurrentDirectory();
            var downloaded = new List<string>();
            foreach (var url in _mUrls)
            {
                if (filteredFiles != null)
                {
                    foreach (var filteredFile in filteredFiles)
                    {
                        var currentFilename = new Wildcard(GetFilename(filteredFile), RegexOptions.IgnoreCase);
                        var currentExt = new Wildcard(GetExtension(filteredFile), RegexOptions.IgnoreCase);

                        Debug.Assert(url.StartsWith("/repository/download/"));

                        // figure out local filename
                        var parts = url.Split('/').Skip(5).ToArray();
                        var destination = flatten
                            ? parts.Last()
                            : string.Join(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture), parts);
                        destination = Path.Combine(directory, destination);


                        if (currentFilename.IsMatch(Path.GetFileNameWithoutExtension(destination)) &&
                            currentExt.IsMatch(Path.GetExtension(destination)))
                        {
                            var directoryName = Path.GetDirectoryName(destination);
                            if (directoryName != null && !Directory.Exists(directoryName))
                                Directory.CreateDirectory(directoryName);

                            downloaded.Add(Path.GetFullPath(destination));

                            if (File.Exists(destination))
                            {
                                if (overwrite) File.Delete(destination);
                                else continue;
                            }

                            var currentUrl = url;
                            if (!string.IsNullOrEmpty(_mParam))
                            {
                                currentUrl = $"{currentUrl}?{_mParam}";
                            }

                            _mCaller.GetDownloadFormat(tempfile => File.Move(tempfile, destination), currentUrl, false);
                            break;
                        }
                    }
                }
            }

            return downloaded;
        }

        private static string GetExtension(string path)
        {
            return path.Substring(path.LastIndexOf('.'));
        }

        private static string GetFilename(string path)
        {
            return path.Substring(0, path.LastIndexOf('.'));
        }
    }

    internal class Wildcard : Regex
    {
        public Wildcard(string pattern)
            : base(WildcardToRegex(pattern))
        {
        }

        public Wildcard(string pattern, RegexOptions options)
            : base(WildcardToRegex(pattern), options)
        {
        }

        public static string WildcardToRegex(string pattern)
        {
            return "^" + Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";
        }
    }
}