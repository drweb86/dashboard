import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  menuItems: { title: string, url: string }[] = [
    {
      title: 'Home',
      url: '/',
    },
    {
      title: 'Dashboard',
      url: '/my-dashboard',
    },
  ];
}
