using System;
using System.Collections.Generic;
using System.Linq;
using IniParser;
using IniParser.Model;
using IniParser.Model.Configuration;
using IniParser.Parser;

namespace BaSta.TimeSync
{
    /// <summary>
    /// Settings factory for <see cref="TimeSyncSettings"/> in a specified INI file.
    /// </summary>
    internal class TimeSyncSettingsFactory
    {
        private const string InputSectionName = "Input";
        private const string OutputSectionNamePrefix = "Output";
        
        /// <summary>
        /// Load a <see cref="TimeSyncSettings"/>  instance obtainable from the INI structure in the specified file.
        /// </summary>
        /// <param name="settingFilePath">The path to the file to load the settings from.</param>
        /// <returns>An instance of <see cref="TimeSyncSettings"/> loaded from the specified file.</returns>
        internal static TimeSyncSettings Load(string settingFilePath)
        {
            // Load INI information from description file
            IniData iniData = new FileIniDataParser(new IniDataParser(new IniParserConfiguration { AllowDuplicateKeys = true })).ReadFile(settingFilePath);
            
            List<ITimeSyncSettingsGroup> outputSettings = new List<ITimeSyncSettingsGroup>();

            // Try to load input section
            if (!iniData.Sections.ContainsSection(InputSectionName))
                throw new Exception($"Missing configuration section '{InputSectionName}'!");

            ITimeSyncSettingsGroup inputSettings = new IniTimeSyncSettingsGroup(iniData.Sections.Single(x => x.SectionName == InputSectionName));

            // Multiple outputs with proper prefix can be configured
            foreach (SectionData section in iniData.Sections.Where(x => x.SectionName.StartsWith(OutputSectionNamePrefix)))
                outputSettings.Add(new IniTimeSyncSettingsGroup(section));

            return new TimeSyncSettings(inputSettings, outputSettings);
        }
    }
}