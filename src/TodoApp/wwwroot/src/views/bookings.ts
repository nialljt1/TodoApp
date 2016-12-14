﻿import { BaseViewModel } from "./baseViewModel";
import { inject } from "aurelia-framework";
import { Router } from 'aurelia-router';
import { bindingMode } from "aurelia-binding";
import { HttpClient, json } from "aurelia-fetch-client";

@inject(BaseViewModel, Router, HttpClient, json)
export class Bookings {
    constructor(baseViewModel: BaseViewModel, router: Router, private http: HttpClient) {
        this.router = router;
        this.baseViewModel = baseViewModel;
    }

    bookings: Array<IBooking>;
    pageSize = 10;
    apiUrl: string;
    bookingFromDate: string;
    bookingToDate: string;
    isCancelled: boolean;
    router: Router;
    baseViewModel: BaseViewModel;

    bind() {
        ////this.apiUrl = "http://localhost:5001/TodoAppApi/Bookings/"
        this.apiUrl = "http://localhost:5001/Bookings/FilterBookings/1"
        this.baseViewModel.setup();
        this.setup();
    }

    setup() {
        var _this = this;
        this.bookingFromDate = null;
        this.bookingToDate = "13/12/2016";
        this.isCancelled = false;

        this.baseViewModel.mgr.getUser().then(function (user) {
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
        this.baseViewModel.mgr.getUser().then(function (user) {

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