import { inject } from "aurelia-framework";
import { HttpClient, json } from "aurelia-fetch-client";

@inject(HttpClient, json)
export class AddBooking {
    firstName: string;
    surname: string;
    emailAddress: string;
    telephoneNumber: string;
    bookingDate: string;
    bookingTime: string;
    numberOfDiners: number;
    startingAt: Date;

    apiUrl: string;
    mgr: Oidc.UserManager;
    message: string;

    constructor(private http: HttpClient) { }

    activate() {
        ////this.apiUrl = "http://localhost:5001/TodoAppApi/Bookings/"
        this.apiUrl = "http://localhost:5001/Bookings/"
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

    addBooking() {
        var _this = this;
        this.mgr.getUser().then(function (user) {
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