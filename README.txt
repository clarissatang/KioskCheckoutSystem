------------------------------
Checkout System Design
------------------------------

1. Created a csv file to store all the product information. The format is
-----------------------------------------------------------------------------------------
Product_Name | Regular_Price | is_On_Sale | Sale_Price | is_Additional_Sale | Sale_Rule
-----------------------------------------------------------------------------------------
Apple        | 1.59          | Yes        | 1.19       | Yes                |Group: buy [3] for $[2]

The column of "Sale_Rule" has to follow the following regulation:
1). Group promotional price based on the quantity purchased
    The template is "Group: buy [how many] for $ [how much]".
    The first parameter indicates the quantity of this group 
    The second parameter indicate the price for this group.
    The parameters must be put in square bracket. 
    e.g. If the promotion is buy 5 for $4, "Sale_Rule" must be Group: buy [5] for $[4]

2). Additional product discount
    The template is "Buy More: buy [how many], get [another how many] [discount percentage] off"
    The first parameter indicates the quantity for regular price.
    The second parameter indicates the quantity which get the sale price.
    The third parameter indicates the discount percentage.
    The parameters must be put in square bracket.
    e.g. If the promotion is buy one get one 50% off, "Sale_Rule" must be Buy More: buy [1], get [1] [50%] off
    NOTE: if get something free, use 100% off
    e.g. buy three get two free, "Sale_Rule" must be Buy More: buy [3], get [2] [100%] off

2. There are three input files for this system.
1) Product_Price.csv stores all the product information. (Details are explained above)
2) Item_List.txt simulates the shopping cart for checkout, each item takes one line.
3) Receipt_Header.txt is the template for receipt printing.

3. Writing unit test is pretty new to me, I wrote a few but I'm not sure whether it is a good unit test.

4. The solution can collect all exceptions and they are saved in Error_Report.txt file.