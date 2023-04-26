import React, {useEffect, useState} from 'react';
import {useDispatch, useSelector} from "react-redux";
import {Chat, Message} from "../models/chat.model";
import {RootState} from "../store";
import {setLoading} from "../store/messengerState";
import styled from "styled-components";

const chatData: Chat = {
        id: 0,
        title: 'chat 1',
        messages: [{
            id: 0,
            content: 'cool first message and it really works cool huh?',
            author: 'Alex',
            mine: true
        },
            {
                id: 1,
                content: 'cool first message and it really works cool huh?',
                author: 'John',
                mine: false
            },
            {
                id: 2,
                content: 'Ahaool huh?',
                author: 'Alex',
                mine: true
            },
            {
                id: 3,
                content: 'cool first message and it really works cool huh?',
                author: 'Nick',
                mine: false
            },
            {
                id: 13,
                content: 'cool first message and it really works cool huh?',
                author: 'John',
                mine: false
            },
            {
                id: 23,
                content: 'Ahaool huh?',
                author: 'Alex',
                mine: true
            },
            {
                id: 33,
                content: 'cool first message and it really works cool huh?',
                author: 'Nick',
                mine: false
            },
            {
                id: 18,
                content: 'cool first message and it really works cool huh?',
                author: 'John',
                mine: false
            },
            {
                id: 28,
                content: 'Ahaool huh?',
                author: 'Alex',
                mine: true
            },
            {
                id: 38,
                content: 'cool first message and it really works cool huh?',
                author: 'Nick',
                mine: false
            },
            {
                id: 284,
                content: 'Ahaool huh?',
                author: 'Alex',
                mine: true
            },
            {
                id: 385,
                content: 'cool first message and it really works cool huh?',
                author: 'Nick',
                mine: false
            },
        ],
    };

const Col = styled.div`
  display: flex;
  flex-direction: column;
  width: 100%;
  height: 100px;
  padding: 1rem;
  border: 1px solid black;
  justify-content: space-evenly;
  border-radius: 6px;
`;

const Title = styled.span`
    font-weight: bold;
`;

const Preview = styled.span`
  color: rgba(black, 0.8);
  text-overflow: ellipsis;
  white-space: nowrap;
  overflow: hidden;
`;

function MessageElem({ msg }: { msg: Message}) {
    return <Col>
        <Title>{ `${msg.author}` }</Title>
        <Preview>{ msg.content }</Preview>
    </Col>
}

const MessagesContainer = styled.div`
  display: flex;
  flex-direction: column-reverse;
  align-items: center;
  overflow-y: auto;
  width: 100%;
`;

function ChatPage() {
    /*const dispatch = useDispatch();
    const [search, setSearch] = useState('');
    const [dialogData, setDialogData] = useState<Message | null>(null);

    const state = useSelector((state: RootState) => state.messenger);

    const getStubData = (additional: boolean, skip?: number) => {
        dispatch(setLoading({additional}));
        //axios.get('/students', { params: {skip, ...(search && search.length >= 3 && { searchTerm: search })} }).then(res =>
            //dispatch(dataLoaded({additional, students: res.data}))
        //).catch(() => dispatch(dataLoaded({ additional, students: [] })));
    }

    useEffect( () => {
        getStubData(false, 0);
    }, [search]);

    const rowClicked = (row: Message) => {
        setDialogData(row);
    }*/

    const [chat, setChat] = useState<Chat>(chatData);

    return <MessagesContainer>{ chat.messages.map(msg => <MessageElem key={msg.id} msg={msg}></MessageElem>) }</MessagesContainer>
}

export default ChatPage;
