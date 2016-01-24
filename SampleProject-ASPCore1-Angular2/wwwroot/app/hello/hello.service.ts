import {Http} from 'angular2/http';
import {Injectable} from 'angular2/core';
import {Observable} from "rxjs";

@Injectable()
export class HelloService {

    private _sayHelloServiceUrl: String = '/api/hello/';

    constructor(private http: Http) { }

    // Gets the activations of an specified license from the server
    public sayHello(name: string): Observable<string> {
        var url = `${this._sayHelloServiceUrl}?name=${name}`;

        return this.http.get(url).map(res => res.json().message);
    }
}