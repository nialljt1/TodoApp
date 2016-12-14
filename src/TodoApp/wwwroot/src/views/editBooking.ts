import { BaseViewModel } from "./baseViewModel";
import { inject } from "aurelia-framework";
import { HttpClient, json } from "aurelia-fetch-client";
import { Router } from 'aurelia-router';

@inject(BaseViewModel, HttpClient, json, Router)
export class EditBooking {
    firstName: string;
    surname: string;
    emailAddress: string;
    telephoneNumber: string;
    bookingDate: string;
    bookingTime: string;
    numberOfDiners: number;
    startingAt: Date;
    baseViewModel: BaseViewModel;

    apiUrl: string;

    constructor(private http: HttpClient) {
    }

    activate() {
        ////this.apiUrl = "http://localhost:5001/TodoAppApi/Bookings/"
        this.apiUrl = "http://localhost:5001/Bookings/"
        this.baseViewModel.setup();
    }

    loadBooking()
    {

    }

    addBooking() {
        var _this = this;
        this.baseViewModel.mgr.getUser().then(function (user) {
            var newBooking = {
                firstName: _this.firstName,
                surname: _this.surname,
                emailAddress: _this.emailAddress,
                telephoneNumber: _this.telephoneNumber,
                startingAt: new Date(_this.bookingDate + " " + _this.bookingTime),
                numberOfDiners: _this.numberOfDiners
            };

            if (user)
            {
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
                method: "post",
                body: json(newBooking)

            }).then(response => {
                console.log("booking added: ", response);
            });

        });    
    }
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