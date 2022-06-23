
export interface PostType {
	userId?: number;
	id?: number;
	title: string;
	body: string;
}

export interface AnimalType {
	id: number;
	avatarUrl: string;
    name: string;
	age: number;
	is_booked: boolean;
	is_liked: boolean;
	about: string;
	address: string
	
}

export interface UserType {
	Id: number
    firstname: string
	lastname: string
    email: string
    phonenumber: string
    gender: string
    city: string
    date_of_birth: string
    avatarUrl: string

}