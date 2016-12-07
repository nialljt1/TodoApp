import { inject } from "aurelia-framework";
import { HttpClient, json } from "aurelia-fetch-client";

@inject(HttpClient, json)
export class AddBooking {
    todoItems: Array<IBooking>;
    dueDateTodoItem: Date;
    nameTodoItem: string;
    apiUrl: string;
    mgr: Oidc.UserManager;
    message: string;

    constructor(private http: HttpClient) { }

    activate() {
        this.apiUrl = "http://localhost/TodoAppApi/Bookings/"
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
                ////_this.fetchAllTodoItems();
            }
            else {
                ////mgr.signinRedirect();
            }
        });
    }


    addNewTodoItem() {
        var _this = this;
        this.mgr.getUser().then(function (user) {
            const newBooking = {
                firstName: this.firstName,
                surname: this.surname,
                emailAddress: this.emailAddress,
                telephoneNumber: this.telephoneNumber,
                bookingDate: this.bookingDate,
                bookingTime: this.bookingTime
            };
            this.http.fetch(this.apiUrl, {
                method: "post",
                body: json(newBooking)

            }).then(response => {
                ////this.fetchAllTodoItems();
                console.log("todo item added: ", response);
            });

        });    
    }

    ////fetchAllTodoItems() {
    ////    var _this = this;
    ////    this.mgr.getUser().then(function (user) {

    ////        _this.http.configure(config => {
    ////            config.withDefaults({
    ////                headers: {
    ////                    'Authorization': "Bearer " + user.access_token
    ////                }
    ////            })
    ////        });

    ////        return _this.http.fetch(_this.apiUrl).
    ////            then(response => response.json()).then(data => {
    ////                _this.todoItems = data;
    ////            });
    ////    });
    ////}
}

export interface IBooking {
    id: number;
    firstName: string;
    Surname: boolean;
    emailAddress: string;
    telephoneNumber: string;
    bookingDate: string;
    bookingTime: string;
}