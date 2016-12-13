import { inject, TaskQueue } from "aurelia-framework";
import { bindingMode } from "aurelia-binding";
import { HttpClient, json } from "aurelia-fetch-client";

@inject(TaskQueue, HttpClient, json)
export class Bookings {
    constructor(taskQueue: TaskQueue, private http: HttpClient) {
        this.taskQueue = taskQueue;
    }

    taskQueue: TaskQueue;
    bookings: Array<IBooking>;
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
                })
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
        this.http.fetch(this.apiUrl + bookingId,
            { method: "delete" }).then(() => { this.fetchBookings(); });
    }
}

export interface IBooking {
    id: number;
    firstName: string;
    surname: boolean;
    emailAddress: string;
    numberOfGuests: number;
    startedAt: string;
    isSelected: boolean;
}