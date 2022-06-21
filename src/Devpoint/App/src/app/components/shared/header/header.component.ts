import { Component, OnInit } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { Developer } from '../../../models/developer';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  constructor(private userService: UserService, private router: Router) {}
  currentUser?: Developer;
  isAuthenticated: boolean = false;

  ngOnInit() {
    this.userService.currentUser.subscribe((userData) => {
      this.currentUser = userData;
    });
    this.userService.isAuthenticated.subscribe((auth) => {
      this.isAuthenticated = auth;
    });
  }

  onLogout() {
    this.userService.purgeAuth();
    this.router.navigateByUrl('/');
  }
}
