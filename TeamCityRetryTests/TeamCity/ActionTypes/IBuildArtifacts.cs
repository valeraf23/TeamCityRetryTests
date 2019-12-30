﻿using System;

namespace TeamCityRetryTests.TeamCity.ActionTypes
{
  public interface IBuildArtifacts
  {
    void DownloadArtifactsByBuildId(string buildId, Action<string> downloadHandler);

    ArtifactWrapper ByBuildConfigId(string buildConfigId, string param="");
  }
}