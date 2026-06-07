import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Navbar  } from './components/navbar/navbar';
import { Chatbot } from './components/chatbot/chatbot';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Navbar, Chatbot],
  template: `
  <app-navbar></app-navbar>
  <router-outlet></router-outlet>
  <app-chatbot></app-chatbot>
  `
})
export class App {
  title = 'GymMaster';
}