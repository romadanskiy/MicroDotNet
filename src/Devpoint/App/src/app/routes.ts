import {
  AboutUsComponent,
  CompanyComponent,
  DeveloperComponent,
  HomeComponent,
  LoginComponent,
  PostComponent,
  PricingComponent,
  ProfileComponent,
  ProjectComponent,
  RegisterComponent,
  SearchComponent,
  SubscriptionsComponent,
  WalletComponent,
} from './components';
import { AddPostComponent } from './components/post/add-post/add-post.component';
import { CreateProjectComponent } from './components/projects/create-project/create-project.component';
import { CreateCompanyComponent } from './components/companies/create-company/create-company.component';
import { EditPostComponent } from './components/post/edit-post/edit-post.component';
import { EditCompanyComponent } from './components/companies/edit-company/edit-company.component';
import { EditProjectComponent } from './components/projects/edit-project/edit-project.component';
import { ProfileEditComponent } from './components/account/profile-edit/profile-edit.component';
import { Routes } from '@angular/router';
import { NoAuthGuard } from './guards/no-auth.guard';
import { AuthGuard } from './guards/auth.guard';
import { Error404Component } from './components/error404/error404.component';
import { Error403Component } from './components/error403/error403.component';

export const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'login', component: LoginComponent, canActivate: [NoAuthGuard] },
  {
    path: 'register',
    component: RegisterComponent,
    canActivate: [NoAuthGuard],
  },
  { path: 'about-us', component: AboutUsComponent },
  { path: 'pricing', component: PricingComponent, canActivate: [AuthGuard] },
  { path: 'search', component: SearchComponent },
  { path: 'developer/:id', component: DeveloperComponent },
  { path: 'project/:id', component: ProjectComponent },
  { path: 'company/:id', component: CompanyComponent },
  { path: 'post/:id', component: PostComponent },
  { path: 'wallet', component: WalletComponent, canActivate: [AuthGuard] },
  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard] },
  {
    path: 'subscriptions',
    component: SubscriptionsComponent,
    canActivate: [AuthGuard],
  },
  { path: 'add-post', component: AddPostComponent, canActivate: [AuthGuard] },
  {
    path: 'create-project',
    component: CreateProjectComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'create-company',
    component: CreateCompanyComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'profile/edit',
    component: ProfileEditComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'company/:id/edit',
    component: EditCompanyComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'project/:id/edit',
    component: EditProjectComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'post/:id/edit',
    component: EditPostComponent,
    canActivate: [AuthGuard],
  },
  { path: '404', component: Error404Component },
  { path: '403', component: Error403Component },
];
