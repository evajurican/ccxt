using ccxt;
namespace Tests;

// PLEASE DO NOT EDIT THIS FILE, IT IS GENERATED AND WILL BE OVERWRITTEN:
// https://github.com/ccxt/ccxt/blob/master/CONTRIBUTING.md#how-to-contribute-code


public partial class testMainClass : BaseTest
{
    public static void testOHLCV(Exchange exchange, object skippedProperties, object method, object entry, object symbol, object now)
    {
        object format = new List<object>() {1638230400000, exchange.parseNumber("0.123"), exchange.parseNumber("0.125"), exchange.parseNumber("0.121"), exchange.parseNumber("0.122"), exchange.parseNumber("123.456")};
        object emptyNotAllowedFor = new List<object>() {0, 1, 2, 3, 4, 5};
        testSharedMethods.assertStructure(exchange, skippedProperties, method, entry, format, emptyNotAllowedFor);
        testSharedMethods.assertTimestampAndDatetime(exchange, skippedProperties, method, entry, now, 0);
        object logText = testSharedMethods.logTemplate(exchange, method, entry);
        //
        assert(isGreaterThanOrEqual(getArrayLength(entry), 6), add("ohlcv array length should be >= 6;", logText));
        if (!isTrue((inOp(skippedProperties, "roundTimestamp"))))
        {
            testSharedMethods.assertRoundMinuteTimestamp(exchange, skippedProperties, method, entry, 0);
        }
        object high = exchange.safeString(entry, 2);
        object low = exchange.safeString(entry, 3);
        testSharedMethods.assertLessOrEqual(exchange, skippedProperties, method, entry, "1", high);
        testSharedMethods.assertGreaterOrEqual(exchange, skippedProperties, method, entry, "1", low);
        testSharedMethods.assertLessOrEqual(exchange, skippedProperties, method, entry, "4", high);
        testSharedMethods.assertGreaterOrEqual(exchange, skippedProperties, method, entry, "4", low);
        assert(isTrue((isEqual(symbol, null))) || isTrue(((symbol is string))), add(add(add("symbol ", symbol), " is incorrect"), logText)); // todo: check with standard symbol check
    }

}