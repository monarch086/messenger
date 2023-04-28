import React, {useEffect, useState} from 'react';
import {useDispatch, useSelector} from "react-redux";
import {Chat, Message} from "../models/chat.model";
import {RootState} from "../store";
import {setLoading} from "../store/messengerState";
import styled from "styled-components";
import axios from "axios";

/*
const chatData: Chat = {
        id: 0,
        title: 'chat 1',
        messages: [
            {
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
    };*/

const Col = styled.div`
  display: flex;
  flex-direction: column;
  width: 50%;
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

const MessagesContainer = styled.div`
  display: flex;
  flex-direction: column-reverse;
  align-items: center;
  overflow-y: auto;
  width: 100%;
`;

const MessagesPage = styled.div`
  display: flex;
  flex-direction: column;
  align-items: stretch;
  width: 100%;
  height: 100%;
`;

const SendPanel = styled.div`
  display: flex;
  align-items: center;
  padding: 1rem;
`;

function MessageElem({ me, msg }: { me: number, msg: { id: string, text: string, senderId: number}}) {
    return <Col style={{ alignSelf: me === msg.senderId ? 'flex-end' : 'flex-start' }}>
        <Preview>{ msg.text }</Preview>
    </Col>
}

function ChatPage({ me, other }: { me: number, other?: number }) {
    const [msg, setMsg] = useState('');
    const [messages, setMessages] = useState<{ id: string, text: string, senderId: number}[]>([]);

    const get = (): void => {
        axios.get(`http://localhost:8080/massages/${me}?friendId=${other}`).then(res => {
                console.log(res.data);
                setMessages(res.data);
            }
        ).catch((err) => console.log(err));
    }

    useEffect( () => {

        axios.get(`http://localhost:8080/chats/${me}`).then(res =>
            console.log(res),
        ).catch(() => console.log('err'));
        get();

    }, []);

    const send = (): void => {
        console.log(msg);

        axios.post('http://localhost:8080/messages', {
            senderId: me,
            receiverId: other,
            text: msg,
        }).then(res => get(),
        ).catch((err) => console.log(err)).finally(() => setMsg(''));
    }

    if (!other) {
        return <span>"Empty"</span>;
    }
    return <MessagesPage>
        <MessagesContainer>{ messages.map(msg => <MessageElem key={msg.id} me={me} msg={msg}></MessageElem>) }</MessagesContainer>
        <SendPanel>
            <input type='text' value={msg} onInput={(val) => setMsg(val.currentTarget.value)}/>
            <div onClick={send}>Send</div>
        </SendPanel>
    </MessagesPage>
}

export default ChatPage;
