import { inject } from "aurelia-framework";
import { HttpClient, json } from "aurelia-fetch-client";

@inject(HttpClient, json)
export class Welcome {
    mgr: Oidc.UserManager;
    constructor(private http: HttpClient) { }

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
    }


    login() {
        this.mgr.signinRedirect();
    }

    register() {
        this.mgr.signinRedirect("http://localhost/IdentityServer2/account/Register");
    }
}