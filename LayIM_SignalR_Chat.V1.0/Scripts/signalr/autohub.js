/*!
 * ASP.NET SignalR JavaScript Library v2.1.2
 * http://signalr.net/
 *
 * Copyright Microsoft Open Technologies, Inc. All rights reserved.
 * Licensed under the Apache 2.0
 * https://github.com/SignalR/SignalR/blob/master/LICENSE.md
 *
 */

/// <reference path="..\..\SignalR.Client.JS\Scripts\jquery-1.6.4.js" />
/// <reference path="jquery.signalR.js" />

/*
    这段代码是系统自动配合后台代码生成的，如果ChatServer 的Hub代码修改了，需要生成一份，修改下面的代码
*/
layui.define(['signalr'], function (exports) {
    window.jQuery = layui.jquery;
    //=====需要修改开始======
    (function ($, window, undefined) {
        /// <param name="$" type="jQuery" />
        "use strict";

        if (typeof ($.signalR) !== "function") {
            throw new Error("SignalR: SignalR is not loaded. Please ensure jquery.signalR-x.js is referenced before ~/signalr/js.");
        }

        var signalR = $.signalR;

        function makeProxyCallback(hub, callback) {
            return function () {
                // Call the client hub method
                callback.apply(hub, $.makeArray(arguments));
            };
        }

        function registerHubProxies(instance, shouldSubscribe) {
            var key, hub, memberKey, memberValue, subscriptionMethod;

            for (key in instance) {
                if (instance.hasOwnProperty(key)) {
                    hub = instance[key];

                    if (!(hub.hubName)) {
                        // Not a client hub
                        continue;
                    }

                    if (shouldSubscribe) {
                        // We want to subscribe to the hub events
                        subscriptionMethod = hub.on;
                    } else {
                        // We want to unsubscribe from the hub events
                        subscriptionMethod = hub.off;
                    }

                    // Loop through all members on the hub and find client hub functions to subscribe/unsubscribe
                    for (memberKey in hub.client) {
                        if (hub.client.hasOwnProperty(memberKey)) {
                            memberValue = hub.client[memberKey];

                            if (!$.isFunction(memberValue)) {
                                // Not a client hub function
                                continue;
                            }

                            subscriptionMethod.call(hub, memberKey, makeProxyCallback(hub, memberValue));
                        }
                    }
                }
            }
        }

        $.hubConnection.prototype.createHubProxies = function () {
            var proxies = {};
            this.starting(function () {
                // Register the hub proxies as subscribed
                // (instance, shouldSubscribe)
                registerHubProxies(proxies, true);

                this._registerSubscribedHubs();
            }).disconnected(function () {
                // Unsubscribe all hub proxies when we "disconnect".  This is to ensure that we do not re-add functional call backs.
                // (instance, shouldSubscribe)
                registerHubProxies(proxies, false);
            });

            proxies['layimHub'] = this.createHubProxy('layimHub');
            proxies['layimHub'].client = {};
            proxies['layimHub'].server = {
                clientSendMsgToClient: function (message) {
                    return proxies['layimHub'].invoke.apply(proxies['layimHub'], $.merge(["ClientSendMsgToClient"], $.makeArray(arguments)));
                },

                clientSendMsgToGroup: function (message) {
                    return proxies['layimHub'].invoke.apply(proxies['layimHub'], $.merge(["ClientSendMsgToGroup"], $.makeArray(arguments)));
                },

                clientToClient: function (fromUserId, toUserId) {
                    return proxies['layimHub'].invoke.apply(proxies['layimHub'], $.merge(["ClientToClient"], $.makeArray(arguments)));
                },

                clientToGroup: function (sid, rid) {
                    return proxies['layimHub'].invoke.apply(proxies['layimHub'], $.merge(["ClientToGroup"], $.makeArray(arguments)));
                }
            };

            return proxies;
        };

        signalR.hub = $.hubConnection("/layim", { useDefaultPath: false });
        $.extend(signalR, signalR.hub.createHubProxies());

    }(window.jQuery, window));
    //=====需要修改结束======
    exports('autohub', window.jQuery.signalR);
});