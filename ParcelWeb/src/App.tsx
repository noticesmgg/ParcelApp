import { licenseKey } from './devextreme-license';
import config from 'devextreme/core/config';
import ParcelGrid from './Parcel/Parcel';
import ParcelView from './ParcelView/ParcelView';

config({ licenseKey });

function App() {
  return (
    <ParcelGrid></ParcelGrid>
    // <ParcelView></ParcelView>
  )
}

export default App
