import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { BehaviorSubject, map, Observable, subscribeOn } from 'rxjs';
import { plainToTyped } from 'type-transformer';
import { Developer } from '../models/developer';
import { Project } from '../models/project';
import { Company } from '../models/company';
import { Post } from '../models/post';
import * as moment from 'moment';
import { HttpParams } from '@angular/common/http';
import { PaymentEntry, PaymentStatus } from '../models/payment-entry';
import { EntityType } from '../models/entity';
import { Subscription } from '../models/subscription';

@Injectable({
  providedIn: 'root',
})
export class AppService {
  _subscriptionLevels: Record<number, string> = {};
  subscriptionLevels: BehaviorSubject<Record<number, string>> =
    new BehaviorSubject<Record<number, string>>({
      0: 'Subscribe',
      1: 'Free',
    });
  constructor(private apiService: ApiService) {}

  getSubscriptionLevelName() {
    return this.subscriptionLevels;
  }

  getSubscriptionLevelNames() {
    return this.apiService.get(`/subscription-levels`);
  }

  public uploadFile(file: File) {
    const formData = new FormData();
    formData.append('file', file, file.name);

    return this.apiService.postRaw(`/storage/upload`, formData);
  }

  public getImagePath(fileName: string) {
    return `${this.apiService.ApiUrl}/storage/${fileName}`;
  }

  public getTags() {
    return this.apiService.get(`/tags`);
  }

  //#region Mappings
  public mapDeveloper(data: Record<string, unknown>): Developer {
    return plainToTyped(data, Developer);
  }

  public mapProject(data: Record<string, unknown>): Project {
    return plainToTyped(data, Project);
  }

  public mapCompany(data: Record<string, unknown>): Company {
    return plainToTyped(data, Company);
  }

  public mapPost(data: Record<string, any>): Post {
    return plainToTyped(
      {
        ...data,
        date: moment(data['date']).format('DD.MM.YYYY'),
      },
      Post,
    );
  }
  //#endregion

  //#region Developers
  public getDeveloper(id: string) {
    return this.apiService
      .get(`/developers/${id}`)
      .pipe(map(this.mapDeveloper));
  }

  public getDeveloperByEmail(email: string) {
    return this.apiService.get(`/users/${email}`).pipe(map(this.mapDeveloper));
  }

  public updateDeveloper(developerId: string, updateDto: any) {
    return this.apiService.put(`/developers/${developerId}/update`, updateDto);
  }

  public getDeveloperTags(id: string) {
    return this.apiService.get(`/developers/${id}/tags`);
  }

  public getDeveloperProjects(id: string) {
    return this.apiService
      .get(`/developers/${id}/projects`)
      .pipe(map((results) => results.map(this.mapProject)));
  }

  public getDeveloperCompanies(id: string) {
    return this.apiService
      .get(`/developers/${id}/companies`)
      .pipe(map((results) => results.map(this.mapCompany)));
  }

  public getDeveloperPosts(
    id: string,
    search?: string,
    take: number = 3,
    skip: number = 0,
  ) {
    const params: Record<string, any> = {
      ...(search ? { search } : {}),
      take,
      skip,
    };

    return this.apiService
      .get(
        `/developers/${id}/posts`,
        new HttpParams({
          fromObject: params,
        }),
      )
      .pipe(
        map((result) => ({
          totalCount: result.totalCount,
          posts: result.posts.map(this.mapPost),
        })),
      );
  }

  public getDevelopers(
    search?: string,
    take: number = 10,
    skip: number = 0,
    follow: boolean = false,
  ) {
    const params: Record<string, any> = {
      ...(search ? { search } : {}),
      take,
      skip,
      isFollow: follow,
    };

    return this.apiService
      .get(
        `/developers/search`,
        new HttpParams({
          fromObject: params,
        }),
      )
      .pipe(
        map((result) => ({
          totalCount: result.totalCount,
          developers: result.result.map(this.mapDeveloper),
        })),
      );
  }

  public getDeveloperNames() {
    return this.apiService.get(`/developers/names`);
  }

  public switchDeveloperFollow(id: string) {
    return this.apiService.post(`/developers/${id}/switchFollow`);
  }
  //#endregion

  //#region Projects
  public getProject(id: string) {
    return this.apiService.get(`/projects/${id}`).pipe(map(this.mapProject));
  }

  public createProject(projectDto: any) {
    return this.apiService.post(`/projects/create`, projectDto);
  }

  public updateProject(projectId: string, updateProjectDto: any) {
    return this.apiService.put(
      `/projects/${projectId}/update`,
      updateProjectDto,
    );
  }

  public getProjectTags(id: string) {
    return this.apiService.get(`/projects/${id}/tags`);
  }

  public getProjectDevelopers(id: string) {
    return this.apiService
      .get(`/projects/${id}/developers`)
      .pipe(map((results) => results.map(this.mapDeveloper)));
  }

  public updateProjectDevelopers(projectId: string, developerIds: string[]) {
    return this.apiService.put(
      `/projects/${projectId}/update/developers`,
      developerIds,
    );
  }

  public getProjectPosts(
    id: string,
    search?: string,
    take: number = 3,
    skip: number = 0,
  ) {
    const params: Record<string, any> = {
      ...(search ? { search } : {}),
      take,
      skip,
    };

    return this.apiService
      .get(
        `/projects/${id}/posts`,
        new HttpParams({
          fromObject: params,
        }),
      )
      .pipe(
        map((result) => ({
          totalCount: result.totalCount,
          posts: result.posts.map(this.mapPost),
        })),
      );
  }

  public getProjects(
    search?: string,
    take: number = 10,
    skip: number = 0,
    follow: boolean = false,
  ) {
    const params: Record<string, any> = {
      ...(search ? { search } : {}),
      take,
      skip,
      isFollow: follow,
    };

    return this.apiService
      .get(
        `/projects`,
        new HttpParams({
          fromObject: params,
        }),
      )
      .pipe(
        map((result) => ({
          totalCount: result.totalCount,
          projects: result.result.map(this.mapProject),
        })),
      );
  }

  public getProjectNames() {
    return this.apiService.get(`/projects/names`);
  }

  public switchProjectFollow(id: string) {
    return this.apiService.post(`/projects/${id}/switchFollow`);
  }
  //#endregion

  //#region Companies
  public getCompany(id: string) {
    return this.apiService.get(`/companies/${id}`).pipe(map(this.mapCompany));
  }

  public createCompany(companyDto: any) {
    return this.apiService.post(`/companies/create`, companyDto);
  }

  public updateCompany(companyId: string, updateCompanyDto: any) {
    return this.apiService.put(
      `/companies/${companyId}/update`,
      updateCompanyDto,
    );
  }

  public getCompanyTags(id: string) {
    return this.apiService.get(`/companies/${id}/tags`);
  }

  public getCompanyDevelopers(id: string) {
    return this.apiService
      .get(`/companies/${id}/developers`)
      .pipe(map((results) => results.map(this.mapDeveloper)));
  }

  public updateCompanyDevelopers(companyId: string, developerIds: string[]) {
    return this.apiService.put(
      `/companies/${companyId}/update/developers`,
      developerIds,
    );
  }

  public getCompanyProjects(id: string) {
    return this.apiService
      .get(`/companies/${id}/projects`)
      .pipe(map((results) => results.map(this.mapProject)));
  }

  public getCompanyPosts(
    id: string,
    search?: string,
    take: number = 3,
    skip: number = 0,
  ) {
    const params: Record<string, any> = {
      ...(search ? { search } : {}),
      take,
      skip,
    };

    return this.apiService
      .get(
        `/companies/${id}/posts`,
        new HttpParams({
          fromObject: params,
        }),
      )
      .pipe(
        map((result) => ({
          totalCount: result.totalCount,
          posts: result.posts.map(this.mapPost),
        })),
      );
  }

  public getCompanies(
    search?: string,
    take: number = 10,
    skip: number = 0,
    follow: boolean = false,
  ) {
    const params: Record<string, any> = {
      ...(search ? { search } : {}),
      take,
      skip,
      isFollow: follow,
    };

    return this.apiService
      .get(
        `/companies`,
        new HttpParams({
          fromObject: params,
        }),
      )
      .pipe(
        map((result) => ({
          totalCount: result.totalCount,
          companies: result.result.map(this.mapCompany),
        })),
      );
  }

  public getCompanyNames() {
    return this.apiService.get(`/companies/names`);
  }

  public switchCompanyFollow(id: string) {
    return this.apiService.post(`/companies/${id}/switchFollow`);
  }
  //#endregion
  withdraw(withdraw: number) {
    return this.apiService.post(`/withdrawals/create`, {
      amount: withdraw.toString(),
    });
  }

  deposit(deposit: number) {
    return this.apiService.post(`/replenishments/create`, {
      amount: deposit.toString(),
    });
  }

  getWalletAmount() {
    return this.apiService
      .get(`/wallets/user`)
      .pipe(map((wallet) => wallet.amount));
  }

  getAllWithdrawals(): Observable<PaymentEntry[]> {
    return this.apiService.get(`/withdrawals`);
  }

  getAllDeposits(): Observable<PaymentEntry[]> {
    return this.apiService.get(`/replenishments`);
  }

  getAllBills(): Observable<PaymentEntry[]> {
    return this.apiService.get('/bills').pipe(
      map((bills) =>
        bills.map((bill: any) => ({
          id: bill.id,
          amount: bill.amount,
          dateTime: bill.dateTime,
          status: bill.status,
          subscriptionType:
            this._subscriptionLevels[bill.tariff.subscriptionLevelId],
        })),
      ),
    );
  }

  public getPost(id: string) {
    return this.apiService.get(`/posts/${id}`).pipe(map(this.mapPost));
  }

  public createPost(postDto: any) {
    return this.apiService.post(`/posts/create`, postDto);
  }

  public updatePost(postId: number, updateDto: any) {
    return this.apiService.put(`/posts/${postId}/update`, updateDto);
  }

  public getSubscription(targetId: string, type: EntityType) {
    const params: Record<string, any> = {
      targetId,
      type,
    };

    return this.apiService
      .get(
        `/subscriptions/find`,
        new HttpParams({
          fromObject: params,
        }),
      )
      .pipe(
        map((sub) => ({
          ...sub,
          entity:
            sub.entityType === EntityType.Developer
              ? this.mapDeveloper(sub.entity)
              : sub.entityType === EntityType.Project
              ? this.mapProject(sub.entity)
              : this.mapCompany(sub.entity),
        })),
      );
  }

  public getProjectSubscriptions() {
    return this.apiService.get('/subscriptions/project').pipe(
      map((subs) =>
        subs.map((sub: any) => ({
          ...sub,
          entity: this.mapProject(sub.entity),
        })),
      ),
    );
  }

  public getDeveloperSubscriptions() {
    return this.apiService.get('/subscriptions/developer').pipe(
      map((subs) =>
        subs.map((sub: any) => ({
          ...sub,
          entity: this.mapDeveloper(sub.entity),
        })),
      ),
    );
  }

  public getCompanySubscriptions() {
    return this.apiService.get('/subscriptions/company').pipe(
      map((subs) =>
        subs.map((sub: any) => ({
          ...sub,
          entity: this.mapCompany(sub.entity),
        })),
      ),
    );
  }

  public createSubscription(
    targetId: string,
    subscriptionLevelId: number,
    type: EntityType,
    isAutoRenewal?: boolean,
  ) {
    const createSubDto = {
      targetId,
      subscriptionLevelId,
      type,
      isAutoRenewal,
    };

    return this.apiService.post(`/subscriptions/create`, createSubDto);
  }

  public updateSubscription(subscriptionId: number, isAutoRenewable: boolean) {
    const updateSubDto = {
      subscriptionId,
      isAutoRenewable,
    };

    return this.apiService.put(`/subscriptions/update`, updateSubDto);
  }

  public cancelSubscription(subscriptionId: number) {
    return this.apiService.delete(`/subscriptions/${subscriptionId}/cancel`);
  }

  getUploadMdUrl() {
    return `${this.apiService.ApiUrl}/storage/upload-md`;
  }
}
