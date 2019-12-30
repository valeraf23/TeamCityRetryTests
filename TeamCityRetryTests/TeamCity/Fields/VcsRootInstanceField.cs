﻿using System;

namespace TeamCityRetryTests.TeamCity.Fields
{
  public class VcsRootInstanceField : IField
  {
    #region Properties

    public bool Id { get; private set; }
    public bool VcsName { get; private set; }
    public bool VcsRootId { get; private set; }
    public bool Href { get; private set; }
    public bool Name { get; private set; }
    public bool Version { get; private set; }
    public bool Status { get; private set; }
    public bool LastChecked { get; private set; }

    public PropertiesField Properties { get; set; }

    #endregion

    #region Public Methods

    public static VcsRootInstanceField WithFields(PropertiesField properties = null,
                                          bool id = false,
                                          bool vcsName = false,
                                          bool vcsrootid = false,
                                          bool href = false,
                                          bool name = false,
                                          bool version = false,
                                          bool status = false,
                                          bool lastChecked = false)
    {
      return new VcsRootInstanceField
      {
          Properties = properties,
          Id = id,
          VcsName = vcsName,
          VcsRootId = vcsrootid,
          Href = href,
          Name = name,
          Version = version,
          Status = status,
          LastChecked = lastChecked
        };
    }

    #endregion

    #region Overrides IField

    public string FieldId
    {
      get { return "vcs-root-instance"; }
    }

    public override string ToString()
    {
      var currentFields = String.Empty;

      FieldHelper.AddField(Id, ref currentFields, "id");
      FieldHelper.AddField(VcsName, ref currentFields, "vcsName");
      FieldHelper.AddField(VcsRootId, ref currentFields, "vcs-root-id");
      FieldHelper.AddField(Href, ref currentFields, "href");
      FieldHelper.AddField(Name, ref currentFields, "name");
      FieldHelper.AddField(Version, ref currentFields, "version");
      FieldHelper.AddField(Status, ref currentFields, "status");
      FieldHelper.AddField(LastChecked, ref currentFields, "lastChecked");

      FieldHelper.AddFieldGroup(Properties, ref currentFields);

      return currentFields;
    }

    #endregion
  }
}