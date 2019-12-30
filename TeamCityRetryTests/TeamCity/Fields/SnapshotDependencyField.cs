﻿using System;

namespace TeamCityRetryTests.TeamCity.Fields
{
  public class SnapshotDependencyField : IField
  {
    #region Properties

    public SourceBuildTypeField SourceBuildType { get; private set; }
    public PropertiesField Properties { get; private set; }
    public bool Id { get; private set; }
    public bool Type { get; private set; }
    public bool Inherited { get; private set; }

    #endregion

    #region Public Methods

    public static SnapshotDependencyField WithFields(SourceBuildTypeField sourceBuildType = null,
      PropertiesField properties = null,
      bool id = false,
      bool type = false,
      bool inherited = false)
    {
      return new SnapshotDependencyField
      {
        SourceBuildType = sourceBuildType,
        Properties = properties,
        Id = id,
        Type = type,
        Inherited = inherited
      };
    }

    #endregion

    #region Overrides IField

    public string FieldId
    {
      get { return "snapshot-dependency"; }
    }

    public override string ToString()
    {
      var currentFields = String.Empty;

      FieldHelper.AddField(Id, ref currentFields, "id");
      FieldHelper.AddField(Type, ref currentFields, "type");
      FieldHelper.AddField(Inherited, ref currentFields, "inherited");

      FieldHelper.AddFieldGroup(SourceBuildType, ref currentFields);
      FieldHelper.AddFieldGroup(Properties, ref currentFields);

      return currentFields;
    }

    #endregion
  }
}