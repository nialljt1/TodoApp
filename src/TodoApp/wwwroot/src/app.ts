import { inject } from "aurelia-framework";
import { Router } from "aurelia-router";

@inject(Router)
export class App {
    router: Router;
    mgr: Oidc.UserManager;
    isLoggedIn: boolean;
    dateValue = null;

    constructor() { }

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

    configureRouter(config, router: Router) {
        this.router = router;

        config.title = "Todo App";
        config.map(
            [
                { route: ["booking"], moduleId: "./views/booking", nav: true, title: "Booking" },
                { route: ["", "welcome"], moduleId: "./views/welcome", nav: true, title: "Welcome" },
                { route: ["help"], moduleId: "./views/help", nav: true, title: "Help" },
                { route: ["about"], moduleId: "./views/about", nav: true, title: "About" },
                { route: ["logout"], moduleId: "./views/logout", nav: true, title: "Log out" },
                { route: ["addBooking"], moduleId: "./views/addBooking", nav: true, title: "Add booking" },
                { name:"editBooking", route: ["editBooking/:id"], moduleId: "./views/editBooking", nav: false, title: "Edit booking" },
                { route: ["bootstrapForm"], moduleId: "./views/bootstrapForm", nav: true, title: "Bootstrap Form" },
                { route: ["bookings"], moduleId: "./views/bookings", nav: true, title: "Bookings" },
            ]);        
    }

    logout() {
        this.mgr.signoutRedirect();
    }
}