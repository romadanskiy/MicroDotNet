import { Component, OnInit, ViewChild } from '@angular/core';
import { Developer } from 'src/app/models/developer';
import { Post } from 'src/app/models/post';
import { Project } from 'src/app/models/project';
import { SwiperComponent } from 'swiper/angular';
import { DevpointMiniPreviewProps } from '@ui-kit/components/devpoint-mini-preview/devpoint-mini-preview.props';
import * as moment from 'moment';
import { plainToTyped } from 'type-transformer';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
})
export class ProfileComponent implements OnInit {
  constructor(private userService: UserService) {}
  currentUser?: Developer;

  ngOnInit(): void {
    this.userService.currentUser.subscribe((userData) => {
      this.currentUser = userData;
    });
  }
}
