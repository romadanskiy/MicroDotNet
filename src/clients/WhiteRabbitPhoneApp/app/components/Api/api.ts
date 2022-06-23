import axios, { AxiosResponse } from 'axios';
import { AnimalType, PostType, UserType } from './post.interface';
//DAL уровень
const instance = axios.create({
	//baseURL: 'http://jsonplaceholder.typicode.com/',
	baseURL: 'http://192.168.88.184:5149/api/',

	timeout: 15000,
});
//чтобы достать только data из response 
const responseBody = (response: AxiosResponse) => response.data;

const requests = {
	get: (url: string) => instance.get(url).then(responseBody),
	post: (url: string, body: {}) => instance.post(url, body).then(responseBody),
	put: (url: string, body: {}) => instance.put(url, body).then(responseBody),
	delete: (url: string) => instance.delete(url).then(responseBody),
};
//для теста со сторонним api
export const Post = {
	getPosts: (): Promise<PostType[]> => requests.get('posts'),
	getAPost: (id: number): Promise<PostType> => requests.get(`posts/${id}`),
	createPost: (post: PostType): Promise<PostType> =>
		requests.post('posts', post),
	updatePost: (post: PostType, id: number): Promise<PostType> =>
		requests.put(`posts/${id}`, post),
	deletePost: (id: number): Promise<void> => requests.delete(`posts/${id}`),
};

export const Animal = {
	getPosts: (): Promise<AnimalType[]> => requests.get('AnimalProfiles'),
	getAPost: (id: number): Promise<AnimalType> => requests.get(`AnimalProfiles/${id}`),
	createPost: (post: AnimalType): Promise<AnimalType> =>
		requests.post('AnimalProfiles', post),
	updatePost: (post: AnimalType, id: number): Promise<AnimalType> =>
		requests.put(`AnimalProfiles/${id}`, post),
	deletePost: (id: number): Promise<void> => requests.delete(`AnimalProfiles/${id}`),
};

export const User = {
	getPosts: (): Promise<UserType[]> => requests.get('UserProfiles'),
	getAPost: (id: number): Promise<UserType> => requests.get(`UserProfiles/${id}`),
	createPost: (post: UserType): Promise<UserType> =>
		requests.post('UserProfiles', post),
	updatePost: (post: UserType, id: number): Promise<UserType> =>
		requests.put(`UserProfiles/${id}`, post),
	deletePost: (id: number): Promise<void> => requests.delete(`UserProfiles/${id}`),
};