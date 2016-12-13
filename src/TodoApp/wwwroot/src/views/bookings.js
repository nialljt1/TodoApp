System.register(["aurelia-framework", "aurelia-fetch-client"], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    };
    var aurelia_framework_1, aurelia_fetch_client_1;
    var Bookings;
    return {
        setters:[
            function (aurelia_framework_1_1) {
                aurelia_framework_1 = aurelia_framework_1_1;
            },
            function (aurelia_fetch_client_1_1) {
                aurelia_fetch_client_1 = aurelia_fetch_client_1_1;
            }],
        execute: function() {
            Bookings = class Bookings {
                constructor(taskQueue, http) {
                    this.http = http;
                    this.pageSize = 10;
                    this.taskQueue = taskQueue;
                }
                bind() {
                    ////this.apiUrl = "http://localhost:5001/TodoAppApi/Bookings/"
                    this.apiUrl = "http://localhost:5001/Bookings/GetBookings/1";
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
                    var mgr = this.mgr;
                    var _this = this;
                    this.mgr.getUser().then(function (user) {
                        if (user) {
                            _this.fetchBookings();
                        }
                    });
                }
                fetchBookings() {
                    var _this = this;
                    this.mgr.getUser().then(function (user) {
                        _this.http.configure(config => {
                            config.withDefaults({
                                headers: {
                                    'Authorization': "Bearer " + user.access_token
                                }
                            });
                        });
                        return _this.http.fetch(_this.apiUrl).
                            then(response => response.json()).then(data => {
                            $('#example2').hide();
                            _this.bookings = data;
                            // TODO: Resolve timing issue in table
                            setTimeout(function () {
                                $('#example2').DataTable();
                                $('#example2').show();
                            }, 500);
                        });
                    });
                }
                deleteBooking(bookingId) {
                    this.http.fetch(this.apiUrl + bookingId, { method: "delete" }).then(() => { this.fetchBookings(); });
                }
            };
            Bookings = __decorate([
                aurelia_framework_1.inject(aurelia_framework_1.TaskQueue, aurelia_fetch_client_1.HttpClient, aurelia_fetch_client_1.json)
            ], Bookings);
            exports_1("Bookings", Bookings);
        }
    }
});
//# sourceMappingURL=bookings.js.map