
// Start Date
let
    Source = Date.StartOfYear(List.Min(#"SSBC Monthly Sales Logs"[Date]))
in
    Source

// End Date
    let
    Source = Date.EndOfYear(List.Max(#"SSBC Monthly Sales Logs"[Date]))
in
    Source

// Product Offering
let
    Source = Pdf.Tables(File.Contents("C:\Users\hp\Documents\Preparing and Modelling Data\source-files\Source Files\SSBC Product Offerings.pdf"), [Implementation="1.3"]),
    Table001 = Source{[Id="Table001"]}[Data],
    #"Promoted Headers" = Table.PromoteHeaders(Table001, [PromoteAllScalars=true]),
    #"Changed Type" = Table.TransformColumnTypes(#"Promoted Headers",{{"PKProductID", type text}, {"ProductName", type text}, {"Product Type", type text}, {"Serving Amount", Int64.Type}, {"ABV", Percentage.Type}, {"IBU", Int64.Type}, {"Description", type text}}),
    #"Removed Columns" = Table.RemoveColumns(#"Changed Type",{"Description"}),
    #"Removed Blank Rows" = Table.SelectRows(#"Removed Columns", each not List.IsEmpty(List.RemoveMatchingItems(Record.FieldValues(_), {"", null})))
in
    #"Removed Blank Rows"

// CFO_Cost Price
let
    Source = Excel.Workbook(File.Contents("C:\Users\hp\Documents\Preparing and Modelling Data\source-files\Source Files\CFO Metrics Tracker.xlsx"), null, true),
    CFO_CostPrice_Table = Source{[Item="CFO_CostPrice",Kind="Table"]}[Data],
    #"Changed Type" = Table.TransformColumnTypes(CFO_CostPrice_Table,{{"PKProductID", type text}, {"Per Unit Cost to make", type number}, {"Per Unit Sales price", type number}, {"BeerAdvocate Score", Int64.Type}, {"Cost Rank", Int64.Type}, {"Sales Rank", Int64.Type}}),
    #"Changed Type1" = Table.TransformColumnTypes(#"Changed Type",{{"Per Unit Cost to make", Currency.Type}, {"Per Unit Sales price", Currency.Type}})
in
    #"Changed Type1"

// Product Offerings
let
    Source = Pdf.Tables(File.Contents("C:\Users\hp\Documents\Preparing and Modelling Data\source-files\Source Files\SSBC Product Offerings.pdf"), [Implementation="1.3"]),
    Table001 = Source{[Id="Table001"]}[Data],
    #"Promoted Headers" = Table.PromoteHeaders(Table001, [PromoteAllScalars=true]),
    #"Changed Type" = Table.TransformColumnTypes(#"Promoted Headers",{{"PKProductID", type text}, {"ProductName", type text}, {"Product Type", type text}, {"Serving Amount", Int64.Type}, {"ABV", Percentage.Type}, {"IBU", Int64.Type}, {"Description", type text}}),
    #"Removed Columns" = Table.RemoveColumns(#"Changed Type",{"Description"}),
    #"Removed Blank Rows" = Table.SelectRows(#"Removed Columns", each not List.IsEmpty(List.RemoveMatchingItems(Record.FieldValues(_), {"", null})))
in
    #"Removed Blank Rows"

// SSBC Product Cost Profile
let
    Source = Table.NestedJoin(#"Product Offerings", {"PKProductID"}, CFO_CostPrice, {"PKProductID"}, "CFO_CostPrice", JoinKind.LeftOuter),
    #"Expanded CFO_CostPrice" = Table.ExpandTableColumn(Source, "CFO_CostPrice", {"PKProductID", "Per Unit Cost to make", "Per Unit Sales price", "BeerAdvocate Score", "Cost Rank", "Sales Rank"}, {"CFO_CostPrice.PKProductID", "CFO_CostPrice.Per Unit Cost to make", "CFO_CostPrice.Per Unit Sales price", "CFO_CostPrice.BeerAdvocate Score", "CFO_CostPrice.Cost Rank", "CFO_CostPrice.Sales Rank"}),
    #"Removed Columns" = Table.RemoveColumns(#"Expanded CFO_CostPrice",{"CFO_CostPrice.PKProductID"}),
    #"Renamed Columns" = Table.RenameColumns(#"Removed Columns",{{"CFO_CostPrice.Per Unit Cost to make", "Per Unit Cost to make"}, {"CFO_CostPrice.Per Unit Sales price", "Per Unit Sales price"}, {"CFO_CostPrice.BeerAdvocate Score", "BeerAdvocate Score"}, {"CFO_CostPrice.Cost Rank", "Cost Rank"}, {"CFO_CostPrice.Sales Rank", "Sales Rank"}, {"ProductName", "PRODUCT NAME"}})
in
    #"Renamed Columns"

// SSBC Monthly Sales Log
let
    Source = Folder.Files("C:\Users\hp\Documents\Preparing and Modelling Data\source-files\Source Files\Monthly Sales Logs"),
    #"Filtered Hidden Files1" = Table.SelectRows(Source, each [Attributes]?[Hidden]? <> true),
    #"Expanded Attributes" = Table.ExpandRecordColumn(#"Filtered Hidden Files1", "Attributes", {"Content Type", "Kind", "Size", "ReadOnly", "Hidden", "System", "Directory", "Archive", "Device", "Normal", "Temporary", "SparseFile", "ReparsePoint", "Compressed", "Offline", "NotContentIndexed", "Encrypted", "ChangeTime", "SymbolicLink", "MountPoint"}, {"Attributes.Content Type", "Attributes.Kind", "Attributes.Size", "Attributes.ReadOnly", "Attributes.Hidden", "Attributes.System", "Attributes.Directory", "Attributes.Archive", "Attributes.Device", "Attributes.Normal", "Attributes.Temporary", "Attributes.SparseFile", "Attributes.ReparsePoint", "Attributes.Compressed", "Attributes.Offline", "Attributes.NotContentIndexed", "Attributes.Encrypted", "Attributes.ChangeTime", "Attributes.SymbolicLink", "Attributes.MountPoint"}),
    #"Added Custom" = Table.AddColumn(#"Expanded Attributes", "Custom", each Excel.Workbook([Content])),
    #"Expanded Custom" = Table.ExpandTableColumn(#"Added Custom", "Custom", {"Name", "Data", "Item", "Kind", "Hidden"}, {"Custom.Name", "Custom.Data", "Custom.Item", "Custom.Kind", "Custom.Hidden"}),
    #"Filtered Rows" = Table.SelectRows(#"Expanded Custom", each ([Custom.Name] = "Apr 2021 Sales" or [Custom.Name] = "Aug 2021 Sales" or [Custom.Name] = "Dec 2020 Sales" or [Custom.Name] = "Feb 2021 Sales " or [Custom.Name] = "Jan 2021 Sales " or [Custom.Name] = "Jul 2021 Sales" or [Custom.Name] = "Jun 2021 Sales" or [Custom.Name] = "Mar 2021 Sales" or [Custom.Name] = "May 2021 Sales" or [Custom.Name] = "Nov 2020 Sales" or [Custom.Name] = "Oct 2020 Sales " or [Custom.Name] = "Sep 2021 Sales")),
    #"Expanded Custom.Data" = Table.ExpandTableColumn(#"Filtered Rows", "Custom.Data", {"Column1", "Column2", "Column3", "Column4", "Column5", "Column6", "Column7"}, {"Custom.Data.Column1", "Custom.Data.Column2", "Custom.Data.Column3", "Custom.Data.Column4", "Custom.Data.Column5", "Custom.Data.Column6", "Custom.Data.Column7"}),
    #"Removed Other Columns" = Table.SelectColumns(#"Expanded Custom.Data",{"Custom.Data.Column1", "Custom.Data.Column2", "Custom.Data.Column3", "Custom.Data.Column4", "Custom.Data.Column5"}),
    #"Promoted Headers" = Table.PromoteHeaders(#"Removed Other Columns", [PromoteAllScalars=true]),
    #"Changed Type" = Table.TransformColumnTypes(#"Promoted Headers",{{"CustID", type text}, {"ProdID", type text}, {"Date", type any}, {"Currency", type text}, {"Qty", type any}}),
    #"Removed Blank Rows" = Table.SelectRows(#"Changed Type", each not List.IsEmpty(List.RemoveMatchingItems(Record.FieldValues(_), {"", null}))),
    #"Changed Type1" = Table.TransformColumnTypes(#"Removed Blank Rows",{{"Date", type date}}),
    #"Removed Errors" = Table.RemoveRowsWithErrors(#"Changed Type1", {"Date"}),
    #"Renamed Columns" = Table.RenameColumns(#"Removed Errors",{{"Qty", "Quantity"}, {"ProdID", "PKProductID"}, {"CustID", "PKCustomerID"}})
in
    #"Renamed Columns"

// SSBC Fiscal Calender

let
    Source = {Number.From(#"Start Date")..Number.From(#"End Date")},
    #"Converted to Table" = Table.FromList(Source, Splitter.SplitByNothing(), null, null, ExtraValues.Error),
    #"Changed Type" = Table.TransformColumnTypes(#"Converted to Table",{{"Column1", type date}}),
    #"Renamed Columns" = Table.RenameColumns(#"Changed Type",{{"Column1", "Date"}}),
    #"Inserted Year" = Table.AddColumn(#"Renamed Columns", "Year", each Date.Year([Date]), Int64.Type),
    #"Inserted Month" = Table.AddColumn(#"Inserted Year", "Month", each Date.Month([Date]), Int64.Type),
    #"Inserted Month Name" = Table.AddColumn(#"Inserted Month", "Month Name", each Date.MonthName([Date]), type text),
    #"Inserted Quarter" = Table.AddColumn(#"Inserted Month Name", "Quarter", each Date.QuarterOfYear([Date]), Int64.Type),
    #"Inserted Week of Year" = Table.AddColumn(#"Inserted Quarter", "Week of Year", each Date.WeekOfYear([Date]), Int64.Type),
    #"Inserted Day Name" = Table.AddColumn(#"Inserted Week of Year", "Day Name", each Date.DayOfWeekName([Date]), type text),
    #"Inserted First Characters" = Table.AddColumn(#"Inserted Day Name", "First Characters", each Text.Start([Month Name], 3), type text),
    #"Renamed Columns1" = Table.RenameColumns(#"Inserted First Characters",{{"First Characters", "Month-Short"}}),
    #"Inserted Merged Column" = Table.AddColumn(#"Renamed Columns1", "Period", each Text.Combine({[#"Month-Short"], Text.From([Year], "en-US")}, "-"), type text),
    #"Added Custom" = Table.AddColumn(#"Inserted Merged Column", "Month Number", each Text.PadStart(Text.From([Month]),2,"0")),
    #"Added Custom1" = Table.AddColumn(#"Added Custom", "Fiscal Period", each if[Month]>=FyStart
then [Month] - (FyStart-1)
else [Month] + (12 - FyStart + 1)),
    #"Added Custom2" = Table.AddColumn(#"Added Custom1", "Fiscal Year", each if[Month]<FyStart
then [Year]
else [Year]+1),
    #"Reordered Columns" = Table.ReorderColumns(#"Added Custom2",{"Date", "Year", "Month", "Fiscal Period", "Fiscal Year", "Month Name", "Quarter", "Week of Year", "Day Name", "Month-Short", "Period", "Month Number"}),
    #"Inserted Division" = Table.AddColumn(#"Reordered Columns", "Division", each [Fiscal Period] / 3, type number),
    #"Rounded Up" = Table.TransformColumns(#"Inserted Division",{{"Division", Number.RoundUp, Int64.Type}}),
    #"Renamed Columns2" = Table.RenameColumns(#"Rounded Up",{{"Division", "Fiscal Quarter"}}),
    #"Added Custom3" = Table.AddColumn(#"Renamed Columns2", "Fiscal Quarter-Year", each Text.PadStart(Text.From([Fiscal Quarter]),2,"Q")& " - " &Text.From([Fiscal Year])),
    #"Renamed Columns3" = Table.RenameColumns(#"Added Custom3",{{"Fiscal Quarter-Year", "Fiscal Qtr"}}),
    #"Sorted Rows" = Table.Sort(#"Renamed Columns3",{{"Month Number", Order.Ascending}})
in
    #"Sorted Rows"

// SSBC Customer List (as of FY2021)
let
    Source = Csv.Document(File.Contents("C:\Users\hp\Documents\Preparing and Modelling Data\source-files\Source Files\Customer List (as of FY2021).txt"),[Delimiter="	", Columns=6, Encoding=1252, QuoteStyle=QuoteStyle.None]),
    #"Changed Type" = Table.TransformColumnTypes(Source,{{"Column1", type text}, {"Column2", type text}, {"Column3", type text}, {"Column4", type text}, {"Column5", type text}, {"Column6", type text}}),
    #"Promoted Headers" = Table.PromoteHeaders(#"Changed Type", [PromoteAllScalars=true]),
    #"Replaced Value" = Table.ReplaceValue(#"Promoted Headers","Tsting","Tasting",Replacer.ReplaceText,{"CustType"}),
    #"Changed Type1" = Table.TransformColumnTypes(#"Replaced Value",{{"PKCustomerID", type text}, {"Customer", type text}, {"CustType", type text}, {"City", type text}, {"State/Province", type text}, {"Country", type text}}),
    #"Replaced Value1" = Table.ReplaceValue(#"Changed Type1","Barn","Bar",Replacer.ReplaceText,{"CustType"}),
    #"Renamed Columns" = Table.RenameColumns(#"Replaced Value1",{{"CustType", "Customer Type"}})
in
    #"Renamed Columns"