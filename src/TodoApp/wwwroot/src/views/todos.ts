import { inject } from "aurelia-framework";
import { HttpClient, json } from "aurelia-fetch-client";

@inject(HttpClient, json)
export class Todos {
    todoItems: Array<ITodoItem>;
    dueDateTodoItem: Date;
    nameTodoItem: string;
    apiUrl: string;

    constructor(private http: HttpClient) { }

    activate() {
        this.apiUrl = "http://192.168.0.7/TodoApp/api/todos/"
        this.fetchAllTodoItems();
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