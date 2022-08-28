﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Umbraco.Redirects.Import.Models;
using System;
using System.Collections.Generic;

namespace Skybrud.Umbraco.Redirects.Import.Importers {

    public abstract class ImporterBase<TOptions, TResult> : IImporter<TOptions, TResult> where TOptions : IImportOptions where TResult : IImportResult {

        #region Properties

        /// <summary>
        /// Gets the type of the importer.
        /// </summary>
        [JsonProperty("type", Order = -99)]
        public string Type => GetType().AssemblyQualifiedName;

        /// <summary>
        /// Gets the icon of the importer.
        /// </summary>
        [JsonProperty("icon", Order = -98)]
        public string Icon { get; protected set; } = "icon-binarycode";

        /// <summary>
        /// Gets the name of the importer.
        /// </summary>
        [JsonProperty("name", Order = -97)]
        public string Name { get; protected set; }

        /// <summary>
        /// Gets the description of the importer.
        /// </summary>
        [JsonProperty("description", Order = -96)]
        public string Description { get; protected set; }

        #endregion

        #region Member methods

        public virtual IEnumerable<Option> GetOptions(HttpRequest request) {
            return Array.Empty<Option>();
        }

        IImportOptions IImporter.ParseOptions(JObject config) {
            return ParseOptions(config);
        }

        public virtual TOptions ParseOptions(JObject config) {
            return config.ToObject<TOptions>();
        }

        IImportResult IImporter.Import(IImportOptions options) {
            if (options is not TOptions t) throw new ArgumentException($"Must be an instance of '{typeof(TOptions)}'", nameof(options));
            return Import(t);
        }

        public abstract TResult Import(TOptions options);

        #endregion

    }

}