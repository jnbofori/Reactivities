import { Dimmer, Loader } from 'semantic-ui-react'

interface Props {
  inverted?: boolean;
  content: string;
} 

export default function LoadingComponent({ inverted = true, content = 'Loading...' }: Props) {
  return (
    <Dimmer inverted={inverted} active={true}>
      <Loader content={content} />
    </Dimmer>
  )
}