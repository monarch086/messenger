import React, {useEffect, useState} from 'react';
import {useDispatch, useSelector} from 'react-redux';
import {RootState} from "./store";
import {dataLoaded, setLoading} from "./store/messengerState";
import styled from "styled-components";
import {Message} from "./models/chat.model";
import ChatsList from "./components/chats-list.component";
import ChatPage from "./components/chat-page.component";


const RootContainer = styled.div`
  height: 100%;
  width: 100%;
  display: flex;
`;

const ChatsListContainer = styled.div`
  flex: 30% 0 0;
  max-width: 350px;
`;

function App() {
    const dispatch = useDispatch();
    const [search, setSearch] = useState('');
    const [dialogData, setDialogData] = useState<Message | null>(null);

    const state = useSelector((state: RootState) => state.messenger);

    const getStubData = (additional: boolean, skip?: number) => {
        dispatch(setLoading({additional}));
        /*axios.get('/students', { params: {skip, ...(search && search.length >= 3 && { searchTerm: search })} }).then(res =>
            dispatch(dataLoaded({additional, students: res.data}))
        ).catch(() => dispatch(dataLoaded({ additional, students: [] })));*/
    }

    useEffect( () => {
        getStubData(false, 0);
    }, [search]);

    return <RootContainer>
        <ChatsListContainer>
            <ChatsList></ChatsList>
        </ChatsListContainer>
        <ChatPage></ChatPage>
    </RootContainer>
}

export default App;
