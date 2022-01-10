# CSV Sandbox

This repository is a place to experiment with CSV.

There are 2 primary components - a parser and a converter

The usage and limitations of both the parser and converter are specified in the unit tests, but below you will find a few highlights.

This project uses the MIT license.

## Parsing

`Parser.Parse(input)` parses a CSV string into a list of lists of strings.
The inner lists represent rows from the input string.
The outer list is the full list of rows.
The parser does not attempt to impart meaning to the CSV. It merely splits an input string into rows and fields.


The parser understands strings that are surrounded by double-quotes ("), but will strip off the double-quotes when populating the field value.

Although this project focuses on CSV, the default comma separator can be changed to any separator via an optional `Settings` object.

The `TrimWhitespace` settings flag tells the parser to remove whitespace around field values.

If the parser encounters an invalid input, it will throw an exception unless you provide an error callback function.

## Converting

The converting API is meant to mimic the Newtonsoft.JSON API (in a spiritual successor sense, not an exact replica).

The CSV header is based on the field and property names. The spelling and casing will be exact copies of the field and property names. You can specify a different spelling with the `CsvProperty` attribute (e.g. `[CsvProperty(PropertyName = "id")]`).

You can change the separator in the `CsvConvertSettings` object.

`CsvConvert.Serialize` and `CsvConvert.SerializeList` are generic functions that serialize an object or list of objects respectively to a CSV representation.

The serialization will ignore non-public fields and properties. Additionally, you can use the `CsvIgnore` attribute to ignore public fields or properties.

`CsvConvert.Deserialize` and `CsvConvert.DeserializeList` are generic function that deserialize a string to an object or list of objects respectively.

The deserialization will not populate non-public fields and properties. Additionally, you can use the `CsvIgnore` attribute to ignore public fields or properties.

If a header row is not available in the input string, the deserialization will fail with an exception unless an error callback function is provided in the `CsvConvertSettings` object.