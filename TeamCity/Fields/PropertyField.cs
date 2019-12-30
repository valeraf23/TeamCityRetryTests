﻿using System;

namespace TeamCityRetryTests.TeamCity.Fields
{
  public class PropertyField : IField
  {
    public bool Name { get; private set; }
    public bool Value { get; private set; }
    public bool Inherited { get; private set; }

    public static PropertyField WithFields(bool name = false, bool value = false, bool inherited = false)
    {
      return new PropertyField
        {
          Name = name,
          Value = value,
          Inherited = inherited
        };
    }

    public string FieldId
    {
      get { return "property"; }
    }

    public override string ToString()
    {
      var currentFields = String.Empty;

      FieldHelper.AddField(Name, ref currentFields, "name");
      FieldHelper.AddField(Value, ref currentFields, "value");
      FieldHelper.AddField(Inherited, ref currentFields, "inherited");
      return currentFields;
    }
  }
}