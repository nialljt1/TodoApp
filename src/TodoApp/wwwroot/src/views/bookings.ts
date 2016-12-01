import { inject } from "aurelia-framework";
import { HttpClient, json } from "aurelia-fetch-client";

@inject(HttpClient, json)
export class Bookings {
    constructor(private http: HttpClient) { }
 
}