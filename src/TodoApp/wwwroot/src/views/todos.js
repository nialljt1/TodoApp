System.register(["aurelia-framework", "aurelia-fetch-client"], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    };
    var aurelia_framework_1, aurelia_fetch_client_1;
    var Todos;
    return {
        setters:[
            function (aurelia_framework_1_1) {
                aurelia_framework_1 = aurelia_framework_1_1;
            },
            function (aurelia_fetch_client_1_1) {
                aurelia_fetch_client_1 = aurelia_fetch_client_1_1;
            }],
        execute: function() {
            Todos = class Todos {
                constructor(http) {
                    this.http = http;
                }
                activate() {
                    this.apiUrl = "http://localhost/TodoAppApi/Todos/";
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
                        else {
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
                        body: aurelia_fetch_client_1.json(newTodoItem)
                    }).then(response => {
                        this.fetchAllTodoItems();
                        console.log("todo item added: ", response);
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
                            });
                        });
                        return _this.http.fetch(_this.apiUrl).
                            then(response => response.json()).then(data => {
                            _this.todoItems = data;
                        });
                    });
                }
                deleteTodoItem(todoItemId) {
                    this.http.fetch(this.apiUrl + todoItemId, { method: "delete" }).then(() => { this.fetchAllTodoItems(); });
                }
                log(message) {
                    this.message = '';
                }
                login() {
                    this.mgr.signinRedirect();
                }
                api() {
                    this.mgr.getUser().then(function (user) {
                        var url = "http://localhost/TodoAppApi/Todos";
                        var xhr = new XMLHttpRequest();
                        xhr.open("GET", url);
                        xhr.onload = function () {
                            ////user.log(xhr.status +  JSON.parse(xhr.responseText));
                        };
                        xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
                        xhr.send();
                    });
                }
                logout() {
                    this.mgr.signoutRedirect();
                }
                markTodoItemAsDone(todoItem) {
                    if (todoItem.isCompleted)
                        return;
                    this.http.fetch(this.apiUrl + todoItem.id, { method: "put" }).then(() => { this.fetchAllTodoItems(); });
                }
            };
            Todos = __decorate([
                aurelia_framework_1.inject(aurelia_fetch_client_1.HttpClient, aurelia_fetch_client_1.json)
            ], Todos);
            exports_1("Todos", Todos);
        }
    }
});
//# sourceMappingURL=todos.js.map