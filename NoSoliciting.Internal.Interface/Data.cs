﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace NoSoliciting.Internal.Interface {
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class Data {
        [LoadColumn(0)]
        public string? Category { get; set; }

        [LoadColumn(1)]
        public uint Channel { get; set; }

        [LoadColumn(2)]
        public string Message { get; set; } = null!;

        public Data() {
        }

        public Data(ushort channel, string message) {
            this.Channel = channel;
            this.Message = message;
        }

        #region computed

        [CustomMappingFactoryAttribute("Compute")]
        [SuppressMessage("ReSharper", "UnusedType.Global")]
        public class ComputeContext : CustomMappingFactory<Data, Computed> {
            private Dictionary<string, float> Weights { get; }

            public ComputeContext() {
                this.Weights = new Dictionary<string, float>();
            }

            public ComputeContext(Dictionary<string, float> weights) {
                this.Weights = weights;
            }

            private void Compute(Data data, Computed computed) {
                data.Compute(computed, this.Weights);
            }

            public override Action<Data, Computed> GetMapping() {
                return this.Compute;
            }
        }

        private static readonly Regex[] PlotWords = {
            new(@"\bplot\b", RegexOptions.Compiled | RegexOptions.IgnoreCase),
            new(@"\bapartment\b", RegexOptions.Compiled | RegexOptions.IgnoreCase),
            new(@"\bapt\b", RegexOptions.Compiled | RegexOptions.IgnoreCase),
            new(@"p.{0,2}\d", RegexOptions.Compiled | RegexOptions.IgnoreCase),
        };

        private static readonly Regex[] WardWords = {
            new(@"\bward\b", RegexOptions.Compiled | RegexOptions.IgnoreCase),
            new(@"w.{0,2}\d", RegexOptions.Compiled | RegexOptions.IgnoreCase),
        };

        private static readonly Regex NumbersRegex = new(@"\d{1,2}.{0,2}\d{1,2}", RegexOptions.Compiled);

        private static readonly string[] TradeWords = {
            "B> ",
            "S> ",
            "buy",
            "sell",
            "WTB",
            "WTS",
        };

        private static readonly Regex SketchUrlRegex = new(@"\.com-\w+\.\w+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public class Computed {
            public float Weight { get; set; } = 1;

            public bool PartyFinder { get; set; }

            public bool Shout { get; set; }

            public bool ContainsWard { get; set; }

            public bool ContainsPlot { get; set; }

            public bool ContainsHousingNumbers { get; set; }

            public bool ContainsTradeWords { get; set; }

            public bool ContainsSketchUrl { get; set; }
        }

        private void Compute(Computed output, IReadOnlyDictionary<string, float> weights) {
            if (this.Category != null && weights.TryGetValue(this.Category, out var weight)) {
                output.Weight = weight;
            }

            output.PartyFinder = this.Channel == 0;
            output.Shout = this.Channel == 11 || this.Channel == 30;
            output.ContainsWard = WardWords.Any(word => word.IsMatch(this.Message));
            output.ContainsPlot = PlotWords.Any(word => word.IsMatch(this.Message));
            output.ContainsHousingNumbers = NumbersRegex.IsMatch(this.Message);
            output.ContainsTradeWords = TradeWords.Any(word => this.Message.ContainsIgnoreCase(word));
            output.ContainsSketchUrl = SketchUrlRegex.IsMatch(this.Message);
        }

        #endregion
    }

    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class Prediction {
        [ColumnName("PredictedLabel")]
        public string Category { get; set; } = "UNKNOWN";

        [ColumnName("Score")]
        public float[] Probabilities { get; set; } = new float[0];
    }

    internal static class Ext {
        public static bool ContainsIgnoreCase(this string haystack, string needle) {
            return CultureInfo.InvariantCulture.CompareInfo.IndexOf(haystack, needle, CompareOptions.IgnoreCase) >= 0;
        }
    }
}