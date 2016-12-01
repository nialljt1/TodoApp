System.register(["aurelia-framework", "aurelia-router"], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    };
    var aurelia_framework_1, aurelia_router_1;
    var App;
    return {
        setters:[
            function (aurelia_framework_1_1) {
                aurelia_framework_1 = aurelia_framework_1_1;
            },
            function (aurelia_router_1_1) {
                aurelia_router_1 = aurelia_router_1_1;
            }],
        execute: function() {
            App = class App {
                constructor() {
                }
                activate() {
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
                    var isLoggedIn = false;
                    var _this = this;
                    this.mgr.getUser().then(function (user) {
                        if (user) {
                            _this.isLoggedIn = true;
                        }
                        else {
                            _this.isLoggedIn = false;
                        }
                    });
                }
                configureRouter(config, router) {
                    this.router = router;
                    config.title = "Todo App";
                    config.map([
                        { route: ["booking"], moduleId: "./views/booking", nav: true, title: "Booking" },
                        { route: ["", "welcome"], moduleId: "./views/welcome", nav: true, title: "Welcome" },
                        { route: ["help"], moduleId: "./views/help", nav: true, title: "Help" },
                        { route: ["about"], moduleId: "./views/about", nav: true, title: "About" },
                        { route: ["logout"], moduleId: "./views/logout", nav: true, title: "Log out" },
                        { route: ["addBooking"], moduleId: "./views/addBooking", nav: true, title: "Add booking" },
                        { route: ["bootstrapForm"], moduleId: "./views/bootstrapForm", nav: true, title: "Bootstrap Form" },
                        { route: ["bookings"], moduleId: "./views/bookings", nav: true, title: "Bookings" },
                    ]);
                }
                logout() {
                    this.mgr.signoutRedirect();
                }
            };
            App = __decorate([
                aurelia_framework_1.inject(aurelia_router_1.Router)
            ], App);
            exports_1("App", App);
        }
    }
});
//# sourceMappingURL=app.js.map