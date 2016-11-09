import { inject } from "aurelia-framework";
import { HttpClient, json } from "aurelia-fetch-client";

@inject(HttpClient, json)
export class Todos {
    todoItems: Array<ITodoItem>;
    dueDateTodoItem: Date;
    nameTodoItem: string;
    apiUrl: string;
    mgr: Oidc.UserManager;
    message: string;

    constructor(private http: HttpClient) { }

    activate() {
        this.apiUrl = "http://localhost/TodoAppApi/Todos/"
        this.fetchAllTodoItems();
        this.setup();
    }

    setup() {
        var config = {
            authority: "http://localhost/IdentityServer",
            client_id: "js",
            redirect_uri: "http://localhost:61039/src/callback.html",
            response_type: "id_token token",
            scope: "openid profile api1",
            post_logout_redirect_uri: "http://localhost:61039/index.html",
        };
        this.mgr = new Oidc.UserManager(config);

        this.mgr.getUser().then(function (user) {
            if (user) {
                alert("User logged in" + user.profile);
            }
            else {
                alert("User not logged in");
            }
        });
    }

    addNewTodoItem() {
        const newTodoItem = {
            DueDate: this.dueDateTodoItem,
            Name: this.nameTodoItem
        };
        this.http.fetch(this.apiUrl, {
            method: "post",
            body: json(newTodoItem)

        }).then(response => {
            this.fetchAllTodoItems();
            console.log("todo item added: ", response);
        });
    }

    fetchAllTodoItems() {
        return this.http.fetch(this.apiUrl).
            then(response => response.json()).then(data => {
                this.todoItems = data;
            });
    }

    deleteTodoItem(todoItemId) {
        this.http.fetch(this.apiUrl + todoItemId,
            { method: "delete" }).then(() => { this.fetchAllTodoItems(); });
    }

    log(message: string) {
        this.message = '';
    }

    login1()  {
        this.mgr.signinRedirect();
    }

    api() {
        this.mgr.getUser().then(function (user) {
            var url = "http://localhost/TodoAppApi/Todos";

            var xhr = new XMLHttpRequest();
            xhr.open("GET", url);
            xhr.onload = function () {
                ////user.log(xhr.status +  JSON.parse(xhr.responseText));
            }
            xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
            xhr.send();
        });
    }

    logout() {
        this.mgr.signoutRedirect();
    }

    markTodoItemAsDone(todoItem: ITodoItem) {
        if (todoItem.isCompleted) return;
        this.http.fetch(this.apiUrl + todoItem.id,
            { method: "put" }).then(() => { this.fetchAllTodoItems(); });
    }
}

export interface ITodoItem {
    id: number;
    name: string;
    isCompleted: boolean;
    createdAt: Date;
    dueDate: Date;
}