﻿SELECT PRODUCT_NAME, PRODUCT_PRICE, STOCK_IN, CASE WHEN STOCK_IN < 100 THEN 'Critical' ELSE 'Not Critical' END AS critical_level FROM viewtblinventoryrecords WHERE STOCK_IN < 100;