﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NoSoliciting {
    public static class RMTUtil {
        private static readonly Dictionary<char, string> replacements = new Dictionary<char, string>() {
            // alphabet
            ['\ue070'] = "?",
            ['\ue071'] = "A",
            ['\ue072'] = "B",
            ['\ue073'] = "C",
            ['\ue074'] = "D",
            ['\ue075'] = "E",
            ['\ue076'] = "F",
            ['\ue077'] = "G",
            ['\ue078'] = "H",
            ['\ue079'] = "I",
            ['\ue07a'] = "J",
            ['\ue07b'] = "K",
            ['\ue07c'] = "L",
            ['\ue07d'] = "M",
            ['\ue07e'] = "N",
            ['\ue07f'] = "O",
            ['\ue080'] = "P",
            ['\ue081'] = "Q",
            ['\ue082'] = "R",
            ['\ue083'] = "S",
            ['\ue084'] = "T",
            ['\ue085'] = "U",
            ['\ue086'] = "V",
            ['\ue087'] = "W",
            ['\ue088'] = "X",
            ['\ue089'] = "Y",
            ['\ue08a'] = "Z",

            // numerals
            ['\ue055'] = "1",
            ['\ue056'] = "2",
            ['\ue057'] = "3",
            ['\ue058'] = "4",
            ['\ue059'] = "5",

            ['\ue060'] = "0",
            ['\ue061'] = "1",
            ['\ue062'] = "2",
            ['\ue063'] = "3",
            ['\ue064'] = "4",
            ['\ue065'] = "5",
            ['\ue066'] = "6",
            ['\ue067'] = "7",
            ['\ue068'] = "8",
            ['\ue069'] = "9",

            ['\ue090'] = "1",
            ['\ue091'] = "2",
            ['\ue092'] = "3",
            ['\ue093'] = "4",
            ['\ue094'] = "5",
            ['\ue095'] = "6",
            ['\ue096'] = "7",
            ['\ue097'] = "8",
            ['\ue098'] = "9",
            ['\ue099'] = "10",
            ['\ue09a'] = "11",
            ['\ue09b'] = "12",
            ['\ue09c'] = "13",
            ['\ue09d'] = "14",
            ['\ue09e'] = "15",
            ['\ue09f'] = "16",
            ['\ue0a0'] = "17",
            ['\ue0a1'] = "18",
            ['\ue0a2'] = "19",
            ['\ue0a3'] = "20",
            ['\ue0a4'] = "21",
            ['\ue0a5'] = "22",
            ['\ue0a6'] = "23",
            ['\ue0a7'] = "24",
            ['\ue0a8'] = "25",
            ['\ue0a9'] = "26",
            ['\ue0aa'] = "27",
            ['\ue0ab'] = "28",
            ['\ue0ac'] = "29",
            ['\ue0ad'] = "30",
            ['\ue0ae'] = "31",

            ['\ue0b1'] = "1",
            ['\ue0b2'] = "2",
            ['\ue0b3'] = "3",
            ['\ue0b4'] = "4",
            ['\ue0b5'] = "5",
            ['\ue0b6'] = "6",
            ['\ue0b7'] = "7",
            ['\ue0b8'] = "8",
            ['\ue0b9'] = "9",

            // symbols
            ['\ue0af'] = "+",

            // letters in other sets
            ['\ue022'] = "A",
            ['\ue024'] = "_A",
            ['\ue0b0'] = "E",
        };

        public static string Normalise(string input) {
            if (input == null) {
                throw new ArgumentNullException(nameof(input), "input cannot be null");
            }

            foreach (KeyValuePair<char, string> entry in replacements) {
                input = input.Replace($"{entry.Key}", entry.Value);
            }
            return input.Normalize(NormalizationForm.FormKD);
        }
    }
}
