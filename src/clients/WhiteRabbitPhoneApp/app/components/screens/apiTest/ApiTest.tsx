import { StatusBar } from 'expo-status-bar';
import React, { FC, useEffect, useState } from 'react';
import { StyleSheet, Text, View, TouchableHighlight } from 'react-native';
import axios from 'axios'
import { Post } from '../../Api/api';
import { PostType } from '../../Api/post.interface';
//для экспериментов с вызовом api
interface EditProps {
	setIsEdit: (state: boolean) => void;
	posts: PostType[]; //add
	setPosts: (updatedPost: PostType[]) => void; //add
	postID: number | null; //add
	setPostID: (id: number) => void; //add
}

const App: FC<EditProps> = ({ setIsEdit, posts, setPosts, postID, setPostID }) =>{
  let [quote, setQuote] = React.useState()
  let [source, setSource] = React.useState('')

  //------------------------------------------------------------------------
  const fetchApiCall = () => {
    fetch("http://ip:5149/api/AnimalProfiles")
      .then(response => response.json())
      .then((responseJson) => {
        console.log('getting data from fetch', responseJson)
        

    })
      .catch(err => {
        console.log(err);
      });
  }
  const axiosApiCall = () => {
    axios({
      "method": "GET",
      "url": "http://ip:5149/api/AnimalProfiles/2",
      
    })
      .then((response) => {
        setQuote(response.data.id);
        setSource(response.data.name);
        console.log('getting data from fetch', response);
      })
      .catch((error) => {
        console.log(error)
      })
  }
  const axiosApiCallnew = () => {
    axios({
      "method": "GET",
      "url": "http://ip:5149/api/AnimalProfiles/2",
      //"url": "http://jsonplaceholder.typicode.com/posts/1",
      
    })
      .then((response) => {
        
        setQuote(response.data.id);
        setSource(response.data.title);
        console.log('getting data from fetch', response);
      })
      .catch((error) => {
        console.log(error)
      })
  }
  //----------------------------------------------------------------------
  const [value, setValue] = useState({
		title: 'new test',
		body: 'bady',
	});
	const [isError, setIsError] = useState<boolean>(false);
//нужно чтобы достать данные из поста с конкретным id 
	useEffect(() => {
		Post.getAPost(postID!)
			.then((data) =>
				setValue({ ...value, title: data.title, body: data.body })
			)
			.catch((err) => setIsError(true));
		return () => {};
	}, []);

	const handleChange = (e: React.FormEvent<EventTarget>) => {
		let target = e.target as HTMLInputElement;
		setValue({ ...value, [target.name]: target.value });
	};

	const handleSubmit = () => {
		//add
		Post.updatePost(value, quote!)
			.then((data) => {
				let updatedPost = posts.filter((post) => post.id !== quote!);
				setPosts([data, ...updatedPost]);
				setValue({ ...value, title: '', body: '' });
			})
			.then((err) => {
				setIsError(true);
			});
	};

 
  const axiosApiCallnewpost = () => {
    axios({
      "method": "GET",
      //"url": "http://ip:5149/api/AnimalProfiles/2",
      "url": "http://jsonplaceholder.typicode.com/posts/1",
      
    })
      .then((response) => {
        setQuote(response.data.id);
        setSource(response.data.name);
        console.log('getting data from fetch', response);
      })
      .catch((error) => {
        console.log(error)
      })
  }
  const res =() => {
    axios ({
      method: 'put',
      url: 'http://jsonplaceholder.typicode.com/posts/1',
    }).then((response) => {
      //setQuote(response.data.id);
      //setSource(title);
      console.log('getting data from fetch', response);
    })   
  }
/*
simple post
  public async addUser(user: any) {
    const response = await axios.post(`/api/user`, {user});
    return response.data;
}
*/
  return (
    <View style={styles.container}>
      <Text style={styles.title}>Native API Calls</Text>
      <Text>Example with fetch and Axios</Text>
      <TouchableHighlight onPress={fetchApiCall}>
        <View style={styles.button}>
          <Text style={styles.buttonText}>Use Fetch API</Text>
        </View>
      </TouchableHighlight>
      <TouchableHighlight onPress={axiosApiCallnew}>
        <View style={styles.button}>
          <Text style={styles.buttonText}>Use Axios</Text>
        </View>
      </TouchableHighlight>
      <TouchableHighlight onPress={handleSubmit}>
        <View style={styles.button}>
          <Text style={styles.buttonText}>Use Axios put</Text>
        </View>
      </TouchableHighlight>
      
      <View>
        <Text>{quote}</Text>
        <Text>{source}</Text>
      </View>
      <StatusBar style="auto" />
    </View>
  );
}
const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#AAA',
    alignItems: 'center',
    justifyContent: 'center',
    color: '#fff'
  },
  title: {
    fontSize: 35,
    color: '#fff'
  },
  button: {
    padding: 10,
    marginVertical: 15,
    backgroundColor: '#0645AD'
  },
  buttonText: {
    color: '#fff'
  }
});
export default App