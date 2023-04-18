import os
import sys

root = os.path.dirname(os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))))
sys.path.append(root)

# ----------------------------------------------------------------------------

# PLEASE DO NOT EDIT THIS FILE, IT IS GENERATED AND WILL BE OVERWRITTEN:
# https://github.com/ccxt/ccxt/blob/master/CONTRIBUTING.md#how-to-contribute-code

# ----------------------------------------------------------------------------
# -*- coding: utf-8 -*-


from ccxt.test.base.test_account import test_account  # noqa E402


async def test_fetch_accounts(exchange):
    method = 'fetchAccounts'
    accounts = await exchange.fetch_accounts()
    assert isinstance(accounts, dict), exchange.id + ' ' + method + ' must return an object. ' + exchange.json(accounts)
    account_values = list(accounts.values())
    for i in range(0, len(account_values)):
        test_account(exchange, method, accounts[i])
