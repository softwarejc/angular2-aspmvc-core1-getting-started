import {Component} from 'angular2/core';
import {OnInit} from 'angular2/core';

import {HelloService} from './hello.service';

@Component({
    // Declare the tag name in index.html to where the component attaches
    selector: 'hello',
    // Location of the template for this component
    templateUrl: 'views/hello.view.html',
    // Dependencies
    providers: [HelloService]
})

// Component controller 
export class Hello implements OnInit {

    public message: string;
    public name: string;

    constructor(private _helloService: HelloService) {
    }

    ngOnInit() {
        this.name = "";
        this.message = "";
    }

    sayHello() {
        // Use hello service to call the API and bind result
        this._helloService.sayHello(this.name).subscribe(response => {
            this.message = response;
        });
    }
}

