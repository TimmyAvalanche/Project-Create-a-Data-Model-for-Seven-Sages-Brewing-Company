# Project: Data-Model-for-Seven-Sages-Brewing-Company
This project focuses on Modelling Data with Power BI

**Objective:** The goal of this step is to create a comprehensive report that summarizes the output of the data model for the CFO of Seven Sages Brewing Company. The report will provide insights into sales performance and gross profit metrics, facilitating informed decision-making.

**Key Components:**

**A. The Report Structure:**
Tab 1: Summarizes sales by customer and customer type across quarters.
Tab 2: Provides a summary of gross profit percentages and unit sales by product.

**B. Visualizations:**
Tab 1: Label the tab "Gross Profit and Unit Sales." > Include a table visual displaying:Product Name, % of Unit Sales by Product, % of Gross Profit by Product
Tab 2: A simple table showing the percentage of sales and gross profit by each product.

**C. Executive Summary:** Each tab will feature a brief Executive Summary at the bottom, providing key insights and findings from the data analysis.

**D. Analysis:** Analyze the table data to identify products that may require further review by the sales team, focusing on those generating significantly higher or lower gross profit relative to sales.

> Outcome: The completed report will enable the CFO to visualize critical sales and profit metrics, supporting strategic decisions regarding marketing and pricing strategies for the brewing company.

Project Completion Process
Step 1: Data Acquisition and Transformation
Power Query M Language.
Ensure that the data model diagram includes a single fact table and four separate dimension tables (currency, customer, product, and CFO metrics).
Step 2: Data Cleaning

Clean the data to remove any obvious typos and errors that could affect reporting functionality.
Verify that the final customer types are accurate, ensuring only three types: "Bar," "Distributor," and "SSBC Tasting Room."
Step 3: Date Table Creation

Create a dynamic date table using Power Query that updates based on the fact table’s start and end dates.
Include standard fields such as continuous calendar dates, month names, fiscal periods, and fiscal years.
Step 4: Building Relationships

Establish one-to-many relationships between each dimension table (the "one" side) and the fact table (the "many" side).
Ensure that all arrows in the data model point towards the fact table.
Step 5: DAX Measures Development

Create key DAX measures that meet reporting requirements, including:
Sales in USD
Cost of Sales in USD
Gross Profit Margin (GPM) in USD
Sales in CAD
Percentage of Unit Sales by Product
Share of Gross Profit by Product Type
Step 6: Report Creation

Design the reporting layer with two tabs:
Tab 1: Include two card visualizations, one matrix, and a text box with an executive summary of key findings.
Tab 2: Create a simple table showing the percentage of sales and gross profit by each product, ensuring the totals equal 100%.
Step 7: Review and Finalization

Review the report for clarity, accuracy, and proper labeling of all measures and visualizations.
Ensure that the layout facilitates easy interpretation of the data and insights for stakeholders.
