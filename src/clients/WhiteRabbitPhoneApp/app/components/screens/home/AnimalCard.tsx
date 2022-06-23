//import { Box, Button, Flex, Image, Link } from '@chakra-ui/react';
import React, { FC } from 'react';
import { View, StyleSheet, TouchableOpacity, Image} from 'react-native';
import { AnimalType } from '../../Api/post.interface';
import { Text, Card } from '@rneui/themed';

interface IAnimalCard {
	//onOpen: () => void;
    id: number;
    name: string;
    age: number;
    avatar: string;
    onLiked: boolean;
    onBooked: boolean;
    onPress: (name: string, avatar: string, onLiked: boolean, onBooked: boolean, id: number) => void
	animal: AnimalType; 
}
//карточка с данными api
const AnimalCard: FC<IAnimalCard> = ({ animal, onPress }) => {
	return (
        <TouchableOpacity onPress={() => onPress(animal.name, animal.avatarUrl, animal.is_liked, animal.is_booked, animal.id)}>
            <Card>
        <Image
        style={styles.image}
        source={{ uri: animal.avatarUrl}}
        />
        <Text style={styles.name} h4>{animal.name}</Text>
        <Text style={styles.name} h4>{animal.age} год(а)</Text>
    </Card>

        </TouchableOpacity>
		
	);
};

const styles = StyleSheet.create({
    container:{
        flex: 1,
    },
    screenTitle:{
        fontSize: 24,
        marginTop: 8,
        fontWeight: 'bold'
    },
    image: {
        width: 150,
        height: 140,
      },
      name: {
        fontSize: 16,
        marginTop: 5,
      },

})

export default AnimalCard;