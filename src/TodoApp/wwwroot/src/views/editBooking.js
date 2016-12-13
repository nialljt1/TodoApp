System.register(["aurelia-framework", "aurelia-fetch-client", 'aurelia-router'], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    };
    var aurelia_framework_1, aurelia_fetch_client_1, aurelia_router_1;
    var EditBooking;
    return {
        setters:[
            function (aurelia_framework_1_1) {
                aurelia_framework_1 = aurelia_framework_1_1;
            },
            function (aurelia_fetch_client_1_1) {
                aurelia_fetch_client_1 = aurelia_fetch_client_1_1;
            },
            function (aurelia_router_1_1) {
                aurelia_router_1 = aurelia_router_1_1;
            }],
        execute: function() {
            EditBooking = class EditBooking {
                constructor(http) {
                    this.http = http;
                }
                activate() {
                    ////this.apiUrl = "http://localhost:5001/TodoAppApi/Bookings/"
                    this.apiUrl = "http://localhost:5001/Bookings/";
                    this.setup();
                }
                setup() {
                    var config = {
                        authority: "http://localhost/IdentityServer2",
                        client_id: "js",
                        redirect_uri: "http://localhost/TodoApp/src/callback.html",
                        response_type: "id_token token",
                        scope: "openid profile api1",
                        post_logout_redirect_uri: "http://localhost/TodoApp/index.html",
                    };
                    this.mgr = new Oidc.UserManager(config);
                }
                loadBooking() {
                }
                addBooking() {
                    var _this = this;
                    this.mgr.getUser().then(function (user) {
                        var newBooking = {
                            firstName: _this.firstName,
                            surname: _this.surname,
                            emailAddress: _this.emailAddress,
                            telephoneNumber: _this.telephoneNumber,
                            startingAt: new Date(_this.bookingDate + " " + _this.bookingTime),
                            numberOfDiners: _this.numberOfDiners
                        };
                        if (user) {
                            _this.http.configure(config => {
                                config
                                    .withDefaults({
                                    headers: {
                                        'Accept': 'application/json',
                                        'X-Requested-With': 'Fetch',
                                        'Authorization': "Bearer " + user.access_token
                                    }
                                });
                            });
                        }
                        _this.http.fetch(_this.apiUrl, {
                            method: "post",
                            body: aurelia_fetch_client_1.json(newBooking)
                        }).then(response => {
                            console.log("booking added: ", response);
                        });
                    });
                }
            };
            EditBooking = __decorate([
                aurelia_framework_1.inject(aurelia_fetch_client_1.HttpClient, aurelia_fetch_client_1.json, aurelia_router_1.Router)
            ], EditBooking);
            exports_1("EditBooking", EditBooking);
        }
    }
});
//# sourceMappingURL=editBooking.js.map