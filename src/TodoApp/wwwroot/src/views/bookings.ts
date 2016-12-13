import { inject, TaskQueue } from "aurelia-framework";
import { Router } from 'aurelia-router';
import { bindingMode } from "aurelia-binding";
import { HttpClient, json } from "aurelia-fetch-client";

@inject(TaskQueue, Router, HttpClient, json)
export class Bookings {
    constructor(taskQueue: TaskQueue, router: Router, private http: HttpClient) {
        this.taskQueue = taskQueue;
        this.router = router;
        debugger;
    }

    taskQueue: TaskQueue;
    bookings: Array<IBooking>;
    pageSize = 10;
    apiUrl: string;
    mgr: Oidc.UserManager;
    message: string;
    bookingFromDate: string;
    bookingToDate: string;
    isCancelled: boolean;
    router: Router;

    bind() {
        ////this.apiUrl = "http://localhost:5001/TodoAppApi/Bookings/"
        this.apiUrl = "http://localhost:5001/Bookings/FilterBookings/1"
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
        this.bookingFromDate = null;
        this.bookingToDate = "13/12/2016";
        this.isCancelled = false;

        this.mgr.getUser().then(function (user) {
            if (user) {
                _this.fetchBookings();
            }
        });
    }

    fetchBookings() {
        var _this = this;
        var filterCriteria = {
            fromDate: _this.bookingFromDate,
            toDate: _this.bookingToDate,
            isCancelled: _this.isCancelled
        };
        this.mgr.getUser().then(function (user) {

            _this.http.configure(config => {
                config.withDefaults({
                    headers: {
                        'Authorization': "Bearer " + user.access_token
                    }
                })
            });

            return _this.http.fetch(_this.apiUrl, {
                method: "POST",
                body: json(filterCriteria)
            }).
                then(response => response.json()).then(data => {
                    $('#example2').hide
                    _this.bookings = data;                                
                    ////$('#example2').DataTable().rows().clear();
                    // TODO: Resolve timing issue in table
                    // TODO: filtering leaves existing data in table - fix
                    setTimeout(function () {
                        $('#example2').DataTable();
                        $('#example2').show();
                    }, 1000);
                });
        });
    }

    deleteBooking(bookingId) {
        this.http.fetch(this.apiUrl + bookingId,
            { method: "delete" }).then(() => { this.fetchBookings(); });
    }

    viewBooking(booking)
    {
        this.router.navigateToRoute('editBooking', { id: booking.id });
    }
}

export interface IBooking {
    id: number;
    firstName: string;
    surname: boolean;
    emailAddress: string;
    numberOfDiners: number;
    startingAt: string;
    isSelected: boolean;
}