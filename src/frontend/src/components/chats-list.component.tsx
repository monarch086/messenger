import React, {useEffect, useState} from 'react';
import {useDispatch, useSelector} from "react-redux";
import {Chat, Message} from "../models/chat.model";
import {RootState} from "../store";
import {setLoading} from "../store/messengerState";
import styled from "styled-components";


const ChatsContainer = styled.aside`
  display: flex;
  flex-direction: column;
  align-items: stretch;
  width: 100%;
  height: 100%;
  border-right: 1px solid black;
  gap: 5px;
  padding: 5px;
`;

const chatsData: Chat[] = [
    {
        id: 0,
        title: 'chat 1',
        /*messages: [{
            id: 0,
            content: 'cool first message and it really works cool huh?'
        }],*/
        other: 10,
        messages: [],
    },
    {
        id: 1,
        title: 'chat 2',
        messages: [],
        other: 20,
    },
    {
        id: 2,
        title: 'chat 3',
        messages: [],
        other: 30,
    }
];

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

function ChatListItem({ chat }: { chat: Chat}) {
    return <Col>
        <Title>{ `${chat.title}(#${chat.id})` }</Title>
        <Preview>{ chat.messages?.length ? chat.messages.slice(-1)[0].content : '...' }</Preview>
    </Col>
}

function ChatsList({ select }: { select: (other: number) => void}) {
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

    const [chats, setChats] = useState<Chat[]>([...chatsData]);

    return <ChatsContainer>
        { chats.map(chat => <div onClick={() => select(chat.other!)}>
            <ChatListItem key={chat.id} chat={chat}></ChatListItem>
        </div> ) }
    </ChatsContainer>;
}

export default ChatsList;
