import type {ReactNode} from 'react';
import clsx from 'clsx';
import Link from '@docusaurus/Link';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import Layout from '@theme/Layout';
import Heading from '@theme/Heading';

import styles from './index.module.css';

type FeatureItem = {
  title: string;
  description: ReactNode;
  link: string;
};

const FeatureList: FeatureItem[] = [
  {
    title: 'SDK',
    description: (
      <>
        A lightweight SDK for defining component stories. Implement the{' '}
        <code>IStory</code> interface, register your assemblies, and let Awen
        discover and render your UI components automatically.
      </>
    ),
    link: '/docs/sdk/story-structure',
  },
  {
    title: 'User Interface',
    description: (
      <>
        A visual explorer for browsing and interacting with your component
        stories. Navigate by category, see live previews, and iterate on your
        Avalonia UI components in real time.
      </>
    ),
    link: '/docs/ui/overview',
  },
];

function Feature({title, description, link}: FeatureItem) {
  return (
    <div className={clsx('col col--6', styles.feature)}>
      <div className="padding-horiz--md padding-vert--md">
        <Heading as="h3">
          <Link to={link}>{title}</Link>
        </Heading>
        <p>{description}</p>
      </div>
    </div>
  );
}

function HomepageHeader() {
  const {siteConfig} = useDocusaurusContext();
  return (
    <header className={clsx('hero hero--primary', styles.heroBanner)}>
      <div className="container">
        <div className={styles.cliBadge}>
          <span className={styles.prompt}>&gt;</span> awen --dir ./stories
        </div>
        <Heading as="h1" className="hero__title">
          {siteConfig.title}
        </Heading>
        <p className="hero__subtitle">{siteConfig.tagline}</p>
        <div className={styles.buttons}>
          <Link
            className="button button--secondary button--lg"
            to="/docs/getting-started/installation">
            Get Started
          </Link>
        </div>
      </div>
    </header>
  );
}

function HomepageFeatures() {
  return (
    <section className={styles.features}>
      <div className="container">
        <div className="row">
          {FeatureList.map((props, idx) => (
            <Feature key={idx} {...props} />
          ))}
        </div>
      </div>
    </section>
  );
}

export default function Home(): ReactNode {
  const {siteConfig} = useDocusaurusContext();
  return (
    <Layout
      title={siteConfig.title}
      description="Storybook for Avalonia UI — develop and preview components in isolation">
      <HomepageHeader />
      <main>
        <HomepageFeatures />
      </main>
    </Layout>
  );
}
