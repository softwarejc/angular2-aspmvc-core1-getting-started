import {bootstrap} from 'angular2/platform/browser'
import {HTTP_PROVIDERS} from 'angular2/http';
import {Hello} from './hello/hello.component';
import 'rxjs/Rx' 

// Bootstrap Angular2 loading Licenses controller
bootstrap(Hello, [HTTP_PROVIDERS]).then(
    success => console.log('app bootstrapped...'),
    error => console.log(error)
);
