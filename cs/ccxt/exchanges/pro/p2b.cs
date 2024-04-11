namespace ccxt.pro;

// PLEASE DO NOT EDIT THIS FILE, IT IS GENERATED AND WILL BE OVERWRITTEN:
// https://github.com/ccxt/ccxt/blob/master/CONTRIBUTING.md#how-to-contribute-code


public partial class p2b { public p2b(object args = null) : base(args) { } }
public partial class p2b : ccxt.p2b
{
    public override object describe()
    {
        return this.deepExtend(base.describe(), new Dictionary<string, object>() {
            { "has", new Dictionary<string, object>() {
                { "ws", true },
                { "cancelAllOrdersWs", false },
                { "cancelOrdersWs", false },
                { "cancelOrderWs", false },
                { "createOrderWs", false },
                { "editOrderWs", false },
                { "fetchBalanceWs", false },
                { "fetchOpenOrdersWs", false },
                { "fetchOrderWs", false },
                { "fetchTradesWs", false },
                { "watchBalance", false },
                { "watchMyTrades", false },
                { "watchOHLCV", true },
                { "watchOrderBook", true },
                { "watchOrders", false },
                { "watchTicker", true },
                { "watchTickers", false },
                { "watchTrades", true },
            } },
            { "urls", new Dictionary<string, object>() {
                { "api", new Dictionary<string, object>() {
                    { "ws", "wss://apiws.p2pb2b.com/" },
                } },
            } },
            { "options", new Dictionary<string, object>() {
                { "OHLCVLimit", 1000 },
                { "tradesLimit", 1000 },
                { "timeframes", new Dictionary<string, object>() {
                    { "15m", 900 },
                    { "30m", 1800 },
                    { "1h", 3600 },
                    { "1d", 86400 },
                } },
                { "watchTicker", new Dictionary<string, object>() {
                    { "name", "state" },
                } },
                { "watchTickers", new Dictionary<string, object>() {
                    { "name", "state" },
                } },
                { "tickerSubs", this.createSafeDictionary() },
            } },
            { "streaming", new Dictionary<string, object>() {
                { "ping", this.ping },
            } },
        });
    }

    public async virtual Task<object> subscribe(object name, object messageHash, object request, object parameters = null)
    {
        /**
        * @ignore
        * @method
        * @description Connects to a websocket channel
        * @param {string} name name of the channel
        * @param {string} messageHash string to look up in handler
        * @param {string[]|float[]} request endpoint parameters
        * @param {object} [params] extra parameters specific to the p2b api
        * @returns {object} data from the websocket stream
        */
        parameters ??= new Dictionary<string, object>();
        object url = getValue(getValue(this.urls, "api"), "ws");
        object subscribe = new Dictionary<string, object>() {
            { "method", name },
            { "params", request },
            { "id", this.milliseconds() },
        };
        object query = this.extend(subscribe, parameters);
        return await this.watch(url, messageHash, query, messageHash);
    }

    public async override Task<object> watchOHLCV(object symbol, object timeframe = null, object since = null, object limit = null, object parameters = null)
    {
        /**
        * @method
        * @name p2b#watchOHLCV
        * @description watches historical candlestick data containing the open, high, low, and close price, and the volume of a market. Can only subscribe to one timeframe at a time for each symbol
        * @see https://github.com/P2B-team/P2B-WSS-Public/blob/main/wss_documentation.md#kline-candlestick
        * @param {string} symbol unified symbol of the market to fetch OHLCV data for
        * @param {string} timeframe 15m, 30m, 1h or 1d
        * @param {int} [since] timestamp in ms of the earliest candle to fetch
        * @param {int} [limit] the maximum amount of candles to fetch
        * @param {object} [params] extra parameters specific to the exchange API endpoint
        * @returns {int[][]} A list of candles ordered as timestamp, open, high, low, close, volume
        */
        timeframe ??= "15m";
        parameters ??= new Dictionary<string, object>();
        await this.loadMarkets();
        object timeframes = this.safeValue(this.options, "timeframes", new Dictionary<string, object>() {});
        object channel = this.safeInteger(timeframes, timeframe);
        if (isTrue(isEqual(channel, null)))
        {
            throw new BadRequest ((string)add(add(this.id, " watchOHLCV cannot take a timeframe of "), timeframe)) ;
        }
        object market = this.market(symbol);
        object request = new List<object>() {getValue(market, "id"), channel};
        object messageHash = add("kline::", getValue(market, "symbol"));
        object ohlcv = await this.subscribe("kline.subscribe", messageHash, request, parameters);
        if (isTrue(this.newUpdates))
        {
            limit = callDynamically(ohlcv, "getLimit", new object[] {symbol, limit});
        }
        return this.filterBySinceLimit(ohlcv, since, limit, 0, true);
    }

    public async override Task<object> watchTicker(object symbol, object parameters = null)
    {
        /**
        * @method
        * @name p2b#watchTicker
        * @description watches a price ticker, a statistical calculation with the information calculated over the past 24 hours for a specific market
        * @see https://github.com/P2B-team/P2B-WSS-Public/blob/main/wss_documentation.md#last-price
        * @see https://github.com/P2B-team/P2B-WSS-Public/blob/main/wss_documentation.md#market-status
        * @param {string} symbol unified symbol of the market to fetch the ticker for
        * @param {object} [params] extra parameters specific to the exchange API endpoint
        * @param {object} [params.method] 'state' (default) or 'price'
        * @returns {object} a [ticker structure]{@link https://docs.ccxt.com/#/?id=ticker-structure}
        */
        parameters ??= new Dictionary<string, object>();
        await this.loadMarkets();
        object watchTickerOptions = this.safeDict(this.options, "watchTicker");
        object name = this.safeString(watchTickerOptions, "name", "state"); // or price
        var nameparametersVariable = this.handleOptionAndParams(parameters, "method", "name", name);
        name = ((IList<object>)nameparametersVariable)[0];
        parameters = ((IList<object>)nameparametersVariable)[1];
        object market = this.market(symbol);
        symbol = getValue(market, "symbol");
        ((IDictionary<string,object>)getValue(this.options, "tickerSubs"))[(string)getValue(market, "id")] = true; // we need to re-subscribe to all tickers upon watching a new ticker
        object tickerSubs = getValue(this.options, "tickerSubs");
        object request = new List<object>(((IDictionary<string,object>)tickerSubs).Keys);
        object messageHash = add(add(name, "::"), getValue(market, "symbol"));
        return await this.subscribe(add(name, ".subscribe"), messageHash, request, parameters);
    }

    public async override Task<object> watchTrades(object symbol, object since = null, object limit = null, object parameters = null)
    {
        /**
        * @method
        * @name p2b#watchTrades
        * @description get the list of most recent trades for a particular symbol
        * @see https://github.com/P2B-team/P2B-WSS-Public/blob/main/wss_documentation.md#deals
        * @param {string} symbol unified symbol of the market to fetch trades for
        * @param {int} [since] timestamp in ms of the earliest trade to fetch
        * @param {int} [limit] the maximum amount of trades to fetch
        * @param {object} [params] extra parameters specific to the exchange API endpoint
        * @returns {object[]} a list of [trade structures]{@link https://docs.ccxt.com/#/?id=public-trades}
        */
        parameters ??= new Dictionary<string, object>();
        await this.loadMarkets();
        object market = this.market(symbol);
        object request = new List<object>() {getValue(market, "id")};
        object messageHash = add("deals::", getValue(market, "symbol"));
        object trades = await this.subscribe("deals.subscribe", messageHash, request, parameters);
        if (isTrue(this.newUpdates))
        {
            limit = callDynamically(trades, "getLimit", new object[] {symbol, limit});
        }
        return this.filterBySinceLimit(trades, since, limit, "timestamp", true);
    }

    public async override Task<object> watchOrderBook(object symbol, object limit = null, object parameters = null)
    {
        /**
        * @method
        * @name p2b#watchOrderBook
        * @description watches information on open orders with bid (buy) and ask (sell) prices, volumes and other data
        * @see https://github.com/P2B-team/P2B-WSS-Public/blob/main/wss_documentation.md#depth-of-market
        * @param {string} symbol unified symbol of the market to fetch the order book for
        * @param {int} [limit] 1-100, default=100
        * @param {object} [params] extra parameters specific to the exchange API endpoint
        * @param {float} [params.interval] 0, 0.00000001, 0.0000001, 0.000001, 0.00001, 0.0001, 0.001, 0.01, 0.1, interval of precision for order, default=0.001
        * @returns {object} A dictionary of [order book structures]{@link https://docs.ccxt.com/#/?id=order-book-structure} indexed by market symbols
        */
        parameters ??= new Dictionary<string, object>();
        await this.loadMarkets();
        object market = this.market(symbol);
        object name = "depth.subscribe";
        object messageHash = add("orderbook::", getValue(market, "symbol"));
        object interval = this.safeString(parameters, "interval", "0.001");
        if (isTrue(isEqual(limit, null)))
        {
            limit = 100;
        }
        object request = new List<object>() {getValue(market, "id"), limit, interval};
        object orderbook = await this.subscribe(name, messageHash, request, parameters);
        return (orderbook as IOrderBook).limit();
    }

    public virtual object handleOHLCV(WebSocketClient client, object message)
    {
        //
        //    {
        //        "method": "kline.update",
        //        "params": [
        //            [
        //                1657648800,             // Kline start time
        //                "0.054146",             // Kline open price
        //                "0.053938",             // Kline close price (current price)
        //                "0.054146",             // Kline high price
        //                "0.053911",             // Kline low price
        //                "596.4674",             // Volume for stock currency
        //                "32.2298758767",        // Volume for money currency
        //                "ETH_BTC"               // Market
        //            ]
        //        ],
        //        "id": null
        //    }
        //
        object data = this.safeList(message, "params");
        data = this.safeList(data, 0);
        object method = this.safeString(message, "method");
        object splitMethod = ((string)method).Split(new [] {((string)".")}, StringSplitOptions.None).ToList<object>();
        object channel = this.safeString(splitMethod, 0);
        object marketId = this.safeString(data, 7);
        object market = this.safeMarket(marketId);
        object timeframes = this.safeDict(this.options, "timeframes", new Dictionary<string, object>() {});
        object timeframe = this.findTimeframe(channel, timeframes);
        object symbol = this.safeString(market, "symbol");
        object messageHash = add(add(channel, "::"), symbol);
        object parsed = this.parseOHLCV(data, market);
        ((IDictionary<string,object>)this.ohlcvs)[(string)symbol] = this.safeValue(this.ohlcvs, symbol, new Dictionary<string, object>() {});
        object stored = this.safeValue(getValue(this.ohlcvs, symbol), timeframe);
        if (isTrue(!isEqual(symbol, null)))
        {
            if (isTrue(isEqual(stored, null)))
            {
                object limit = this.safeInteger(this.options, "OHLCVLimit", 1000);
                stored = new ArrayCacheByTimestamp(limit);
                ((IDictionary<string,object>)getValue(this.ohlcvs, symbol))[(string)timeframe] = stored;
            }
            callDynamically(stored, "append", new object[] {parsed});
            callDynamically(client as WebSocketClient, "resolve", new object[] {stored, messageHash});
        }
        return message;
    }

    public virtual object handleTrade(WebSocketClient client, object message)
    {
        //
        //    {
        //        "method": "deals.update",
        //        "params": [
        //            "ETH_BTC",
        //            [
        //                {
        //                    "id": 4503032979,               // Order_id
        //                    "amount": "0.103",
        //                    "type": "sell",                 // Side
        //                    "time": 1657661950.8487639,     // Creation time
        //                    "price": "0.05361"
        //                },
        //                ...
        //            ]
        //        ],
        //        "id": null
        //    }
        //
        object data = this.safeList(message, "params", new List<object>() {});
        object trades = this.safeList(data, 1);
        object marketId = this.safeString(data, 0);
        object market = this.safeMarket(marketId);
        object symbol = this.safeString(market, "symbol");
        object tradesArray = this.safeValue(this.trades, symbol);
        if (isTrue(isEqual(tradesArray, null)))
        {
            object tradesLimit = this.safeInteger(this.options, "tradesLimit", 1000);
            tradesArray = new ArrayCache(tradesLimit);
            ((IDictionary<string,object>)this.trades)[(string)symbol] = tradesArray;
        }
        for (object i = 0; isLessThan(i, getArrayLength(trades)); postFixIncrement(ref i))
        {
            object item = getValue(trades, i);
            object trade = this.parseTrade(item, market);
            callDynamically(tradesArray, "append", new object[] {trade});
        }
        object messageHash = add("deals::", symbol);
        callDynamically(client as WebSocketClient, "resolve", new object[] {tradesArray, messageHash});
        return message;
    }

    public virtual object handleTicker(WebSocketClient client, object message)
    {
        //
        // state
        //
        //    {
        //        "method": "state.update",
        //        "params": [
        //            "ETH_BTC",
        //            {
        //                "high": "0.055774",         // High price for the last 24h
        //                "close": "0.053679",        // Close price for the last 24h
        //                "low": "0.053462",          // Low price for the last 24h
        //                "period": 86400,            // Period 24h
        //                "last": "0.053679",         // Last price for the last 24h
        //                "volume": "38463.6132",     // Stock volume for the last 24h
        //                "open": "0.055682",         // Open price for the last 24h
        //                "deal": "2091.0038055314"   // Money volume for the last 24h
        //            }
        //        ],
        //        "id": null
        //    }
        //
        // price
        //
        //    {
        //        "method": "price.update",
        //        "params": [
        //            "ETH_BTC",      // market
        //            "0.053836"      // last price
        //        ],
        //        "id": null
        //    }
        //
        object data = this.safeList(message, "params", new List<object>() {});
        object marketId = this.safeString(data, 0);
        object market = this.safeMarket(marketId);
        object method = this.safeString(message, "method");
        object splitMethod = ((string)method).Split(new [] {((string)".")}, StringSplitOptions.None).ToList<object>();
        object messageHashStart = this.safeString(splitMethod, 0);
        object tickerData = this.safeDict(data, 1);
        object ticker = null;
        if (isTrue(isEqual(method, "price.update")))
        {
            object lastPrice = this.safeString(data, 1);
            ticker = this.safeTicker(new Dictionary<string, object>() {
                { "last", lastPrice },
                { "close", lastPrice },
                { "symbol", getValue(market, "symbol") },
            });
        } else
        {
            ticker = this.parseTicker(tickerData, market);
        }
        object symbol = getValue(ticker, "symbol");
        object messageHash = add(add(messageHashStart, "::"), symbol);
        callDynamically(client as WebSocketClient, "resolve", new object[] {ticker, messageHash});
        return message;
    }

    public virtual void handleOrderBook(WebSocketClient client, object message)
    {
        //
        //    {
        //        "method": "depth.update",
        //        "params": [
        //            false,                          // true - all records, false - new records
        //            {
        //                "asks": [                   // side
        //                    [
        //                        "19509.81",         // price
        //                        "0.277"             // amount
        //                    ]
        //                ]
        //            },
        //            "BTC_USDT"
        //        ],
        //        "id": null
        //    }
        //
        object parameters = this.safeList(message, "params", new List<object>() {});
        object data = this.safeDict(parameters, 1);
        object asks = this.safeList(data, "asks");
        object bids = this.safeList(data, "bids");
        object marketId = this.safeString(parameters, 2);
        object market = this.safeMarket(marketId);
        object symbol = getValue(market, "symbol");
        object messageHash = add("orderbook::", getValue(market, "symbol"));
        object subscription = this.safeValue(((WebSocketClient)client).subscriptions, messageHash, new Dictionary<string, object>() {});
        object limit = this.safeInteger(subscription, "limit");
        object orderbook = this.safeValue(this.orderbooks, symbol);
        if (isTrue(isEqual(orderbook, null)))
        {
            ((IDictionary<string,object>)this.orderbooks)[(string)symbol] = this.orderBook(new Dictionary<string, object>() {}, limit);
            orderbook = getValue(this.orderbooks, symbol);
        }
        if (isTrue(!isEqual(bids, null)))
        {
            for (object i = 0; isLessThan(i, getArrayLength(bids)); postFixIncrement(ref i))
            {
                object bid = this.safeValue(bids, i);
                object price = this.safeNumber(bid, 0);
                object amount = this.safeNumber(bid, 1);
                object bookSide = getValue(orderbook, "bids");
                (bookSide as IOrderBookSide).store(price, amount);
            }
        }
        if (isTrue(!isEqual(asks, null)))
        {
            for (object i = 0; isLessThan(i, getArrayLength(asks)); postFixIncrement(ref i))
            {
                object ask = this.safeValue(asks, i);
                object price = this.safeNumber(ask, 0);
                object amount = this.safeNumber(ask, 1);
                object bookside = getValue(orderbook, "asks");
                (bookside as IOrderBookSide).store(price, amount);
            }
        }
        ((IDictionary<string,object>)orderbook)["symbol"] = symbol;
        callDynamically(client as WebSocketClient, "resolve", new object[] {orderbook, messageHash});
    }

    public override void handleMessage(WebSocketClient client, object message)
    {
        if (isTrue(this.handleErrorMessage(client as WebSocketClient, message)))
        {
            return;
        }
        object result = this.safeString(message, "result");
        if (isTrue(isEqual(result, "pong")))
        {
            this.handlePong(client as WebSocketClient, message);
            return;
        }
        object method = this.safeString(message, "method");
        object methods = new Dictionary<string, object>() {
            { "depth.update", this.handleOrderBook },
            { "price.update", this.handleTicker },
            { "kline.update", this.handleOHLCV },
            { "state.update", this.handleTicker },
            { "deals.update", this.handleTrade },
        };
        object endpoint = this.safeValue(methods, method);
        if (isTrue(!isEqual(endpoint, null)))
        {
            DynamicInvoker.InvokeMethod(endpoint, new object[] { client, message});
        }
    }

    public virtual object handleErrorMessage(WebSocketClient client, object message)
    {
        object error = this.safeString(message, "error");
        if (isTrue(!isEqual(error, null)))
        {
            throw new ExchangeError ((string)add(add(this.id, " error: "), this.json(error))) ;
        }
        return false;
    }

    public override object ping(WebSocketClient client)
    {
        /**
         * @see https://github.com/P2B-team/P2B-WSS-Public/blob/main/wss_documentation.md#ping
         * @param client
         */
        return new Dictionary<string, object>() {
            { "method", "server.ping" },
            { "params", new List<object>() {} },
            { "id", this.milliseconds() },
        };
    }

    public virtual object handlePong(WebSocketClient client, object message)
    {
        //
        //    {
        //        error: null,
        //        result: 'pong',
        //        id: 1706539608030
        //    }
        //
        client.lastPong = this.safeInteger(message, "id");
        return message;
    }

    public override void onError(WebSocketClient client, object error)
    {
        ((IDictionary<string,object>)this.options)["tickerSubs"] = this.createSafeDictionary();
        this.onError(client as WebSocketClient, error);
    }

    public override void onClose(WebSocketClient client, object error)
    {
        ((IDictionary<string,object>)this.options)["tickerSubs"] = this.createSafeDictionary();
        this.onClose(client as WebSocketClient, error);
    }
}
