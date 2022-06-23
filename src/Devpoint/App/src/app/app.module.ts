import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UiKitModule } from '@ui-kit';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import {
  HeaderComponent,
  HomeComponent,
  FooterComponent,
  LoginComponent,
  RegisterComponent,
  AboutUsComponent,
  PricingComponent,
  DeveloperPreviewComponent,
  DeveloperComponent,
  SearchComponent,
  ProjectComponent,
  ProjectPreviewComponent,
  CompanyComponent,
  CompanyPreviewComponent,
  ProfileComponent,
  WalletComponent,
  SubscriptionsComponent,
  PostComponent,
  PostPreviewComponent,
  PreviewBaseComponent,
} from './components';
import { SwiperModule } from 'swiper/angular';
import { PostsContainerComponent } from './components/post/posts-container/posts-container.component';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { MarkdownModule } from 'ngx-markdown';
import { MatPaginatorModule } from '@angular/material/paginator';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { AddPostComponent } from './components/post/add-post/add-post.component';
import { SubscriptionPartialComponent } from './components/account/subscriptions/subscription-partial/subscription-partial.component';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDividerModule } from '@angular/material/divider';
import { UnsubscribeConfirmModalComponent } from './components/account/subscriptions/subscription-partial/unsubscribe-confirm-modal/unsubscribe-confirm-modal.component';
import { MatDialogModule } from '@angular/material/dialog';
import { SubscribeConfirmModalComponent } from './components/main/pricing/subscribe-confirm-modal/subscribe-confirm-modal.component';
import { NuMarkdownModule } from '@ng-util/markdown';
import { LocalStorageService } from './services/local-storage.service';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { CreateProjectComponent } from './components/projects/create-project/create-project.component';
import { CreateCompanyComponent } from './components/companies/create-company/create-company.component';
import { EditCompanyComponent } from './components/companies/edit-company/edit-company.component';
import { EditProjectComponent } from './components/projects/edit-project/edit-project.component';
import { EditPostComponent } from './components/post/edit-post/edit-post.component';
import { TextFieldModule } from '@angular/cdk/text-field';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { ProfileEditComponent } from './components/account/profile-edit/profile-edit.component';
import { TagsInputComponent } from './components/tags-input/tags-input.component';
import { DeveloperEntryComponent } from './components/developers/developer-entry/developer-entry.component';
import { DeleteConfirmationComponent } from './components/companies/edit-company/delete-confirmation/delete-confirmation.component';
import { AddDeveloperModalComponent } from './components/companies/edit-company/add-developer-modal/add-developer-modal.component';
import { JwtService } from './services/jwt.service';
import { UserService } from './services/user.service';
import { ApiService } from './services/api.service';
import { AppService } from './services/app.service';
import { HttpTokenInterceptor } from './interceptors/http.token.interceptor';
import { AuthResolver } from './resolvers/auth.resolver';
import { AuthGuard } from './guards/auth.guard';
import { NoAuthGuard } from './guards/no-auth.guard';
import { CustomValidationService } from './services/custom-validation.service';
import { Error404Component } from './components/error404/error404.component';
import { Error403Component } from './components/error403/error403.component';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { FeedComponent } from './components/feed/feed.component';
import {AppMarkdownPreviewComponent} from "./components/app-markdown-preview";

@NgModule({
  imports: [
    HttpClientModule,
    BrowserModule,
    AppRoutingModule,
    UiKitModule,
    SwiperModule,
    InfiniteScrollModule,
    MarkdownModule.forRoot(),
    MatPaginatorModule,
    BrowserAnimationsModule,
    FormsModule,
    MatAutocompleteModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatMenuModule,
    MatProgressSpinnerModule,
    MatDividerModule,
    MatDialogModule,
    NuMarkdownModule.forRoot(),
    MatButtonToggleModule,
    TextFieldModule,
    MatFormFieldModule,
    MatChipsModule,
    MatIconModule,
    MatExpansionModule,
    MatSlideToggleModule,
  ],
  declarations: [
    AppComponent,
    HeaderComponent,
    HomeComponent,
    FooterComponent,
    LoginComponent,
    RegisterComponent,
    AboutUsComponent,
    PricingComponent,
    DeveloperPreviewComponent,
    DeveloperComponent,
    SearchComponent,
    ProjectComponent,
    ProjectPreviewComponent,
    CompanyComponent,
    CompanyPreviewComponent,
    ProfileComponent,
    WalletComponent,
    SubscriptionsComponent,
    PostComponent,
    PostPreviewComponent,
    PreviewBaseComponent,
    PostsContainerComponent,
    AddPostComponent,
    SubscriptionPartialComponent,
    UnsubscribeConfirmModalComponent,
    SubscribeConfirmModalComponent,
    CreateProjectComponent,
    CreateCompanyComponent,
    EditCompanyComponent,
    EditProjectComponent,
    EditPostComponent,
    ProfileEditComponent,
    TagsInputComponent,
    DeveloperEntryComponent,
    DeleteConfirmationComponent,
    AddDeveloperModalComponent,
    Error404Component,
    Error403Component,
    FeedComponent,
    AppMarkdownPreviewComponent
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: HttpTokenInterceptor, multi: true },
    LocalStorageService,
    JwtService,
    UserService,
    ApiService,
    AppService,
    AuthResolver,
    AuthGuard,
    NoAuthGuard,
    CustomValidationService,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
