import React, {useEffect, useState} from 'react';
import {useDispatch, useSelector} from 'react-redux';
import {RootState} from "./store";
import {dataLoaded, setLoading} from "./store/messengerState";
import styled from "styled-components";
import {Message} from "./models/chat.model";


const Container = styled.div`
    height: 70%;
    width: 70%;
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 2rem;
`;

const Columns = styled.div`
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 0.25rem;
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

    const rowClicked = (row: Message) => {
        setDialogData(row);
    }

    return <span>cool</span>
}

export default App;
