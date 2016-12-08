import { inject } from "aurelia-framework";
import { HttpClient, json } from "aurelia-fetch-client";

@inject(HttpClient, json)
export class Bookings {
    constructor(private http: HttpClient) { }

    bookings = [];
    pageSize = 10;
    apiUrl: string;
    mgr: Oidc.UserManager;
    message: string;

    bind() {
        ////this.apiUrl = "http://localhost:5001/TodoAppApi/Bookings/"
        this.apiUrl = "http://localhost:5001/Bookings/GetBookings/1"
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
                _this.fetchAllTodoItems();
            }
        });
    }

    fetchAllTodoItems() {
        var _this = this;
        this.mgr.getUser().then(function (user) {

            _this.http.configure(config => {
                config.withDefaults({
                    headers: {
                        'Authorization': "Bearer " + user.access_token
                    }
                })
            });

            return _this.http.fetch(_this.apiUrl).
                then(response => response.json()).then(data => {
                    _this.bookings = data;
                    debugger;
                });
        });
    }

    fetchBookings() {
        debugger;
        var _this = this;
        this.mgr.getUser().then(function (user) {
            debugger;
                if (user) {
                    _this.http.configure(config => {
                        config
                            .withDefaults({
                                headers: {
                                    'Accept': 'application/json',
                                    'X-Requested-With': 'Fetch',
                                    'Authorization': "Bearer " + user.access_token
                                }
                            })

                    });
            }

                _this.http.fetch(_this.apiUrl, {
                    method: "GET"
                })
                    .then(response => {
                        console.log("booking added: ", response);
                        debugger;                        
                    });
                    ////.then(response => response.json())
                    ////.then(bookings => _this.bookings = bookings);
        });
    }
}